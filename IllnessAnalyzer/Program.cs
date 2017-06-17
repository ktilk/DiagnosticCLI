using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IllnessAnalyzer
{
    public class Program
    {
        private static readonly List<Disease> Diseases = new List<Disease>();
        private static readonly List<string> UniqueSymptoms = new List<string>();
        [STAThread]
        public static void Main(string[] args)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            //Console.Write(ofd.FileName);
            var file = File.ReadAllLines(ofd.FileName);
            foreach (var line in file)
            {
                var disease = new Disease();
                var words = line.Split(',');
                disease.Name = words[0];
                for (int i = 1; i < words.Length; i++)
                {
                    disease.Symptoms.Add(words[i]);
                    if (!UniqueSymptoms.Contains(words[i])) UniqueSymptoms.Add(words[i]); //check if symptom is new
                }
                Diseases.Add(disease);
                //Console.WriteLine(line);

            }
            Console.WriteLine("Number of diseases: "+Diseases.Count);
            var diseasesWithMostSymptoms = Diseases.OrderByDescending(d => d.Symptoms.Count).ThenBy(d => d.Name).Take(3);
            foreach (var disease in diseasesWithMostSymptoms)
            {
                Console.WriteLine("Disease: " + disease.Name + "(" + disease.Symptoms.Count + " symptoms)");
            }
            Console.WriteLine("Number of unique symptoms: " + UniqueSymptoms.Count);

            Console.ReadLine();
        }
    }
}
