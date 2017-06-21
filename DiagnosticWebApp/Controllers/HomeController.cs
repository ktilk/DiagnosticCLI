using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DiagnosticWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertData(HttpPostedFileBase data)
        {
            var csvReader = new StreamReader(data.InputStream);
            var line = csvReader.ReadLine();
            var values = line.Split();
            return Index();
        }

    }
}