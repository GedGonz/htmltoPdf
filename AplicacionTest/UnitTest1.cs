using System;
using Aplicacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicacionTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var resultado = new ReportServicio().html();
        }
    }
}
