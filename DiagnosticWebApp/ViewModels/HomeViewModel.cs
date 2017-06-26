using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiagnosticWebApp.Models;

namespace DiagnosticWebApp.ViewModels
{
    public class IndexHomeViewModel
    {
        public List<Disease> Diseases { get; set; }
        public List<Symptom> Symptoms { get; set; }
        public List<Symptom> MostPopularSymptoms { get; set; }
        public List<Disease> DiseasesWithMostSymptoms { get; set; }
        public List<Disease> PatientDiseases { get; set; }
    }

    public class DiagnoseHomeViewModel
    {
        public List<Disease> Diseases { get; set; }
        public List<Symptom> Symptoms { get; set; }
        public List<Symptom> MostPopularSymptoms { get; set; }
        public List<Disease> DiseasesWithMostSymptoms { get; set; }
        public List<Disease> PatientDiseases { get; set; }
    }
}
