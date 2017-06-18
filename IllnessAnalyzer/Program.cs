using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiagnosticCLI
{
    public class Program
    {
        private static readonly List<Disease> Diseases = new List<Disease>();
        private static readonly List<Symptom> Symptoms = new List<Symptom>(); // holds all the different symptoms
        [STAThread]
        public static void Main(string[] args)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var file = File.ReadAllLines(ofd.FileName);
            foreach (var line in file)
            {
                var disease = new Disease();
                var words = line.Split(',');
                disease.DiseaseName = words[0];
                for (var i = 1; i < words.Length; i++)
                {
                    var symp = Symptoms.FirstOrDefault(s => s.SymptomName == words[i]);
                    //disease.Symptoms.Add(new Symptom(){SymptomName = words[i]});
                    if (symp == null) // create symptom if does not exist
                    {
                        symp = new Symptom(){ SymptomName = words[i]};
                        Symptoms.Add(symp);
                    }
                    symp.OccurenceRate++;
                    disease.Symptoms.Add(symp);
                }
                Diseases.Add(disease);

            }
            Console.WriteLine("Number of diseases: "+Diseases.Count);
            var diseasesWithMostSymptoms = Diseases.OrderByDescending(d => d.Symptoms.Count)
                .ThenBy(d => d.DiseaseName)
                .Take(3);
            foreach (var disease in diseasesWithMostSymptoms)
            {
                Console.WriteLine("Disease: " + disease.DiseaseName + "(" + disease.Symptoms.Count + " symptoms)");
            }
            Console.WriteLine("Number of unique symptoms: " + Symptoms.Count);
            //foreach (var symptom in Symptoms)
            //{
            //    Console.WriteLine("Symptom '" + symptom.SymptomName + "' number of occurrences: " + symptom.OccurenceRate);
            //}
            var mostPopularSymptoms = Symptoms.OrderByDescending(s => s.OccurenceRate)
                .ThenBy(s => s.SymptomName)
                .Take(3);
            foreach (var symptom in mostPopularSymptoms)
            {
                Console.WriteLine("Symptom: " + symptom.SymptomName + "(" + symptom.OccurenceRate + " occurrences)");
            }


            var patientSymptoms = Console.ReadLine().Split(','); // get patient symptoms
            var patientDiseases = new List<Disease>();
            //var patientDiseases =
                //Diseases.Where(disease => disease.Symptoms.All(x => patientSymptoms.Contains(x.SymptomName)));
                //Diseases.Where(disease => patientSymptoms.All(x => disease.Symptoms.Any(s => s.SymptomName == x)));
            foreach (var patientDisease in patientDiseases)
            {
                Console.WriteLine("Possible disease: " + patientDisease.DiseaseName);
            }
            Console.ReadLine();
        }
    }
}
