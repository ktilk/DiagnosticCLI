using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticCLI.Models
{
    public class Disease
    {
        public string DiseaseName { get; set; }
        public List<Symptom> Symptoms { get; set; } = new List<Symptom>();
    }
}
