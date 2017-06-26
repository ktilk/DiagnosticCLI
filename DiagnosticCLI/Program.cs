using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiagnosticCLI.Models;

namespace DiagnosticCLI
{
    public class Program
    {
        private static readonly List<Disease> Diseases = new List<Disease>();
        private static readonly List<Symptom> Symptoms = new List<Symptom>(); // all the different symptoms
        [STAThread]
        public static void Main(string[] args)
        {
            ReadAndParseCsv();

            Console.WriteLine("Number of diseases: {0}", Diseases.Count);
            DisplayThreeDiseasesWithMostSymptoms(Diseases);

            Console.WriteLine();
            Console.WriteLine("Number of unique symptoms: {0}", Symptoms.Count);

            DisplayThreeMostPopularSymptoms(Symptoms);

            Console.WriteLine();
            SuggestPossibleDiseasesBySymptoms(Diseases);
            
            Console.WriteLine();
            Console.WriteLine("You have: {0}", DiagnoseDisease(Diseases).DiseaseName);
            Console.ReadLine();
        }

        private static void SuggestPossibleDiseasesBySymptoms(List<Disease> diseases)
        {
            var patientDiseases = GetPossibleDiseases(diseases, GetUserSymptoms());
            Console.Write("Possible disease(s): ");
            foreach (var patientDisease in patientDiseases)
            {
                Console.Write(patientDisease.DiseaseName);
                Console.Write(patientDisease == patientDiseases.Last() ? "." : ", ");
            }
        }

        private static void DisplayThreeDiseasesWithMostSymptoms(List<Disease> diseases)
        {
            var diseasesWithMostSymptoms = diseases.OrderByDescending(d => d.Symptoms.Count)
                .ThenBy(d => d.DiseaseName)
                .Take(3);
            Console.WriteLine("Three diseases with the most symptoms:");
            foreach (var disease in diseasesWithMostSymptoms)
            {
                Console.WriteLine("* {0}({1} symptoms)", disease.DiseaseName, disease.Symptoms.Count);
            }
        }

        private static List<string> GetUserSymptoms()
        {
            var patientSymptoms = "";
            while (string.IsNullOrEmpty(patientSymptoms))
            {
                Console.WriteLine("What symptoms do you have?");
                patientSymptoms = Console.ReadLine();
            }
            return patientSymptoms.Split(',').Select(s => s.Trim()).ToList();
        }

        private static void ReadAndParseCsv()
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var file = File.ReadAllLines(ofd.FileName);
            foreach (var line in file)
            {
                var disease = new Disease();
                var words = line.Split(',').Select(s => s.Trim()).ToArray();
                disease.DiseaseName = words[0];
                for (var i = 1; i < words.Length; i++)
                {
                    words[i] = words[i].Trim();
                    var symp = Symptoms.FirstOrDefault(s => s.SymptomName == words[i]);
                    if (symp == null) // create symptom if does not exist
                    {
                        symp = new Symptom() { SymptomName = words[i] };
                        Symptoms.Add(symp);
                    }
                    disease.Symptoms.Add(symp);
                }
                Diseases.Add(disease);
            }
            CountSymptomOccurrenceRates(Diseases);
        }

        /// <summary>
        /// Take all diseases thats symptoms include symptoms given as parameter
        /// </summary>
        /// <param name="diseases"></param>
        /// <param name="patientSymptoms"></param>
        /// <returns></returns>
        private static List<Disease> GetPossibleDiseases(List<Disease> diseases, List<string> patientSymptoms)
        {
            return diseases.Where(d => patientSymptoms.All(x => d.Symptoms.Contains(Symptoms.FirstOrDefault(s => s.SymptomName == x)))).ToList();
        }

        public static void DisplayThreeMostPopularSymptoms(List<Symptom> symptoms)
        {
            Console.WriteLine("Three most popular symptoms:");
            var mostPopularSymptoms = symptoms.OrderByDescending(s => s.OccurenceRate).ThenBy(s => s.SymptomName).Take(3);
            foreach (var symptom in mostPopularSymptoms)
            {
                Console.WriteLine("* {0}({1} occurrences)", symptom.SymptomName, symptom.OccurenceRate);
            }
        }

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
        /// <summary>
        /// Removes diseases that don't include given symptom.
        /// </summary>
        /// <param name="diseases"></param>
        /// <param name="symptom"></param>
        /// <returns></returns>
        public static List<Disease> RemoveDiseasesWithoutSymptom(List<Disease> diseases, string symptom)
        {
            return diseases.Where(d => d.Symptoms.All(s => s.SymptomName != symptom)).ToList();
        }

        public static Disease DiagnoseDisease(List<Disease> diseases)
        {
            var possibleDiseases = diseases; //diseases the patient could possibly have
            var userSymptoms = new List<string>();
            while (possibleDiseases.Count > 1)
            {
                //get the most probable symptom (that has not been asked yet) from possible diseases 
                var symptom = CountSymptomOccurrenceRates(possibleDiseases).OrderByDescending(s => s.OccurenceRate).FirstOrDefault(s => !userSymptoms.Contains(s.SymptomName)).SymptomName;
                Console.WriteLine("Do you have: {0}? yes/no", symptom);
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "yes":
                        userSymptoms.Add(symptom); //add symptom to user symptoms and get new list of diseases
                        possibleDiseases = GetPossibleDiseases(possibleDiseases, userSymptoms);
                        break;
                    case "no":
                        possibleDiseases = RemoveDiseasesWithoutSymptom(possibleDiseases, symptom); //remove diseases that don't include this symptom
                        break;
                    default: //default answer is "yes"
                        userSymptoms.Add(symptom);
                        possibleDiseases = GetPossibleDiseases(possibleDiseases, userSymptoms);
                        break;
                }
            }
            return possibleDiseases.FirstOrDefault(); //return the only disease in list
        }
    }
}
