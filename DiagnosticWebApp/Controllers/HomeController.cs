using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiagnosticWebApp.Models;

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
        public ActionResult InsertData(HttpPostedFileBase dataFile)
        {
            var diseases = new List<Disease>();
            var symptoms = new List<Symptom>();
            var csvReader = new StreamReader(dataFile.InputStream);
            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();
                var disease = new Disease();
                var words = line.Split().Select(s => s.Trim()).ToArray();
                disease.DiseaseName = words[0];
                for (var i = 1; i < words.Length; i++)
                {
                    words[i] = words[i].Trim();
                    var symp = symptoms.FirstOrDefault(s => s.SymptomName == words[i]);
                    if (symp == null) // create symptom if does not exist
                    {
                        symp = new Symptom() { SymptomName = words[i] };
                        symptoms.Add(symp);
                    }
                    disease.Symptoms.Add(symp);
                }
                diseases.Add(disease);
            }
            return Index(diseases, symptoms);
        }

        private ActionResult Index(List<Disease> diseases, List<Symptom> symptoms)
        {
            //TODO tee andmetega maagiat ja pane lehel kuvama
            throw new NotImplementedException();
        }
    }
}