using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticWebApp.Models
{
    public class Symptom
    {
        public Symptom()
        {
            
        }

        public Symptom(string name)
        {
            SymptomName = name;
        }
        public string SymptomName { get; set; }
        public int OccurenceRate { get; set; }
    }
}
