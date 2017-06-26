using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiagnosticWebApp.Models;

namespace DiagnosticWebApp.Services
{
    public class DiseaseService
    {
        public static List<Symptom> CountSymptomOccurrenceRates(List<Disease> diseases)
        {
            var symptoms = new List<Symptom>();
            foreach (var disease in diseases)
            {
                foreach (var symptom in disease.Symptoms)
                {
                    if (symptoms.FirstOrDefault(s => s.SymptomName == symptom.SymptomName) == null)
                    {
                        symptom.OccurenceRate = 0;
                        symptoms.Add(symptom);
                    }
                    symptom.OccurenceRate++;
                }
            }
            return symptoms;
        }
    }
}