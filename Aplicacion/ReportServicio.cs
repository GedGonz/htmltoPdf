using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Aplicacion
{
    public class ReportServicio
    {
        
        

        public byte[] GeneratePDF(string name)
        {

            MemoryStream stream = null;

            var Html = html();
            string ModifiedFileName = string.Empty;
            using (stream = new MemoryStream())
            {
                StringReader sr = new StringReader(Html);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                ModifiedFileName = name;
                ModifiedFileName = ModifiedFileName.Insert(ModifiedFileName.Length - 4, "1");

                
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                
               
                pdfDoc.Close();

                //PdfReader reader = new PdfReader(stream);

                //PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), PdfWriter.STRENGTH128BITS, "", "", iTextSharp.text.pdf.PdfWriter.AllowPrinting);
                //reader.Close();
                //return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
            return stream.ToArray();
        }

        public byte[] GeneratePDF2(string name)
        {
            byte[] result;
            string Path = AppDomain.CurrentDomain.BaseDirectory;
            Path = Path.Substring(0, Path.Length - 10);
            var Pathcss = string.Format("{0}{1}", Path, "htmltoPdf\\Content\\bootstrap.min.css");
            List<string> cssFile = new List<string>();
            cssFile.Add(Pathcss);
            MemoryStream stream = null;

            string Html = html();
            string ModifiedFileName = string.Empty;
            using (stream = new MemoryStream())
            {
                
                Document pdfDoc = new Document(PageSize.A4, 60f, 60f, 50f, 40f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                writer.PageEvent = new NumPage();
                pdfDoc.Open();
                HtmlPipelineContext htmlcontext = new HtmlPipelineContext(null);

                htmlcontext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                ICSSResolver cssResolver= XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssResolver.AddCssFile(Pathcss, true);
                //cssFile.ForEach(x=>cssResolver.AddCssFile(x,true));

                IPipeline pipeline = new CssResolverPipeline(cssResolver,
                    new HtmlPipeline(htmlcontext, new PdfWriterPipeline(pdfDoc, writer)));


                XMLWorker worker = new XMLWorker(pipeline,true);
                XMLParser xmlparser = new XMLParser(worker);
                xmlparser.Parse(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Html)));

                pdfDoc.Close();

                result = stream.GetBuffer();
                //PdfReader reader = new PdfReader(stream);

                //PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), PdfWriter.STRENGTH128BITS, "", "", iTextSharp.text.pdf.PdfWriter.AllowPrinting);
                //reader.Close();
                //return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
            return result;
        }


        public string html()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory;
            Path = Path.Substring(0, Path.Length - 10);

            var Body = "PartialView.cshtml";

            var PathBody = string.Format("{0}Aplicacion\\{1}", Path, Body);
            var RutaBody = File.ReadAllText(PathBody);


            var result = string.Empty;

            var model = new List<Persona>();
            model.Add(new Persona() { Id = 1, Nombre = "Gerald", Apellido = "Gonzalez" });
            model.Add(new Persona() { Id = 2, Nombre = "Gerald2", Apellido = "Gonzalez2" });

            //Crea una template para usarla en cache
            Razor.GetTemplate(RutaBody, model, "template8");

            //Compila la template
            //Razor.Compile(RutaBody, "template");

            //Corre la template y returna el html
            result = Razor.Run("template8",model);


            /* ORIGINAL
             * 
                var model = Json.Decode("{\"Description\":\"Hello World\"}");
                var template = "<div class=\"helloworld\">@Model.Description</div>";
                const string layout = "<html><body>@RenderBody()</body></html>";

                template = string.Format("{0}{1}", "@{Layout=\"_layout\";}", template);

                using (var service = new TemplateService())
                {
                    service.GetTemplate(layout, null, "_layout");
                    service.GetTemplate(template, model, "template");

                    var result = service.Parse(template, model, null, "page");

                    Console.Write(result);
                    Console.ReadKey();
}
             */


            return result;
        }
    }
}
