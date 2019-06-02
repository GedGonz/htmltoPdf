using Aplicacion;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace htmltoPdf.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Utilizando RazorEngine 
            ReportServicio reportServicio = new ReportServicio();
            ViewBag.html = reportServicio.html();


            //Utilizando Itexchart
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Encabezado()
        {
            return View();
        }

        public ActionResult PrintItextchart()
        {
            var name = "Documento.pdf";
            ReportServicio reportServicio = new ReportServicio();
            return  File(reportServicio.GeneratePDF2(name), "application/pdf", name);
        }


        public ActionResult PrintRotativa()
        {
            string _headerUrl = Url.Action("Encabezado","Home",null,"http");
            ReportServicio reportServicio = new ReportServicio();
            string footer = string.Format("--header-spacing \"50\" --footer-right \" Page: [page]/[toPage]\"  --footer-font-size \"9\" --header-html  \"{0}\" --print-media-type ", _headerUrl);
            var html = reportServicio.html();
            ViewBag.html = html;
            var name = "Documento.pdf";
            var PDFView = new ViewAsPdf("PrintRotativa", html)
            {
                FileName = name,
                //PageOrientation = Orientation.Portrait,
                //MinimumFontSize = 10,
                PageMargins = new Margins(60,20,40,20), 
                PageSize = Size.A4,
                CustomSwitches = footer

            };
            return PDFView;
            //return View();
        }


    }
}