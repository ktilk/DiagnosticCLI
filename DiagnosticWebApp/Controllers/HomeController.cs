using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiagnosticWebApp.Models;
using DiagnosticWebApp.Services;
using DiagnosticWebApp.ViewModels;

namespace DiagnosticWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(IndexHomeViewModel vm)
        {
            if (vm.Diseases == null)
            {
                return RedirectToAction("Upload");
            }
            return View("Index", vm);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase uploadedFile)
        {
            var diseases = new List<Disease>();
            var symptoms = new List<Symptom>();
            Console.WriteLine(Request);
            var file = Request.Files[0];
            var csvReader = new StreamReader(file.InputStream);
            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();
                var disease = new Disease();
                var words = line.Split(',').Select(s => s.Trim()).ToArray();
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
            var vm = new IndexHomeViewModel
            {
                Diseases = diseases,
                Symptoms = symptoms,
                DiseasesWithMostSymptoms = diseases.OrderByDescending(d => d.Symptoms.Count)
                    .ThenBy(d => d.DiseaseName)
                    .Take(3)
                    .ToList(),
                MostPopularSymptoms = DiseaseService.CountSymptomOccurrenceRates(diseases)
                    .OrderByDescending(s => s.OccurenceRate)
                    .ThenBy(s => s.SymptomName)
                    .Take(3)
                    .ToList()
            };
            return Index(vm);
        }

        //public ActionResult DiagnoseDiseases(IndexHomeViewModel vm)
        //{
        //    return View("Index", vm);
        //}

        [HttpPost]
        public ActionResult DiagnoseDiseases(string symptoms, IndexHomeViewModel vm)
        {
            var patientSymptoms = symptoms.Split(',').Select(s => s.Trim()).ToList();
            vm.PatientDiseases = vm.Diseases.Where(d => patientSymptoms.All(x => d.Symptoms.Contains(vm.Symptoms.FirstOrDefault(s => s.SymptomName == x)))).ToList();
            return View("Index", vm);
        }
    }
}
