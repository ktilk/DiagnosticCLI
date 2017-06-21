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
            ReadAndParseCsv();
            Console.WriteLine("Number of diseases: "+Diseases.Count);
            var diseasesWithMostSymptoms = Diseases.OrderByDescending(d => d.Symptoms.Count)
                .ThenBy(d => d.DiseaseName)
                .Take(3);
            Console.WriteLine("Three diseases with the most symptoms:");
            foreach (var disease in diseasesWithMostSymptoms)
            {
                Console.WriteLine("* " + disease.DiseaseName + "(" + disease.Symptoms.Count + " symptoms)");
            }
            Console.WriteLine();
            Console.WriteLine("Number of unique symptoms: " + Symptoms.Count);

            var mostPopularSymptoms = CountSymptomOccurrenceRates(Diseases).OrderByDescending(s => s.OccurenceRate).ThenBy(s => s.SymptomName).Take(3);
            Console.WriteLine("Three most popular symptoms:");
            foreach (var symptom in mostPopularSymptoms)
            {
                Console.WriteLine("* " + symptom.SymptomName + "(" + symptom.OccurenceRate + " occurrences)");
            }

            // ask patient for symptoms
            //TODO validate user input
            Console.WriteLine("What symptoms do you have?");
            var patientSymptoms = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList(); // get patient symptoms
            var patientDiseases = GetPossibleDiseases(Diseases, patientSymptoms);
            foreach (var patientDisease in patientDiseases)
            {
                Console.WriteLine("Possible disease: " + patientDisease.DiseaseName);
            }
            Console.WriteLine("You have: " + DiagnoseDisease(Diseases).DiseaseName);
            Console.ReadLine();
        }

        private static void ReadAndParseCsv()
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var file = File.ReadAllLines(ofd.FileName);
            //var file = File.ReadAllLines("C: \\Users\\KasparTilk\\Desktop\\assignment and Java templates\\database.csv");
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
            var mostPopularSymptoms = symptoms.OrderByDescending(s => s.OccurenceRate).ThenBy(s => s.SymptomName).Take(3);
            foreach (var symptom in mostPopularSymptoms)
            {
                Console.WriteLine("Symptom: " + symptom.SymptomName + "(" + symptom.OccurenceRate + " occurrences)");
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

        public static List<Disease> RemoveDiseasesThatDontIncludeSymptom(List<Disease> diseases, string symptom)
        {
            return diseases.Where(d => d.Symptoms.All(s => s.SymptomName != symptom)).ToList();
        }

        //TODO lahendada rekursiivselt?
        public static Disease DiagnoseDisease(List<Disease> diseases)
        {
            var possibleDiseases = diseases;
            var userSymptoms = new List<string>();
            while (possibleDiseases.Count > 1)
            {
                var symptom = CountSymptomOccurrenceRates(possibleDiseases).OrderByDescending(s => s.OccurenceRate).FirstOrDefault(s => !userSymptoms.Contains(s.SymptomName)).SymptomName;
                Console.WriteLine("Do you have: " + symptom + "? yes/no");
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "yes":
                        userSymptoms.Add(symptom);
                        possibleDiseases = GetPossibleDiseases(possibleDiseases, userSymptoms);
                        break;
                    case "no":
                        possibleDiseases = RemoveDiseasesThatDontIncludeSymptom(possibleDiseases, symptom);
                        break;
                    default:
                        userSymptoms.Add(symptom);
                        possibleDiseases = GetPossibleDiseases(possibleDiseases, userSymptoms);
                        break;
                }
            }
            return possibleDiseases.FirstOrDefault();
        }


    }
}
