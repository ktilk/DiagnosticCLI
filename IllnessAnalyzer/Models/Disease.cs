﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticCLI
{
    public class Disease
    {
        //public int Id { get; set; }
        public string DiseaseName { get; set; }
        public List<string> Symptoms { get; set; } = new List<string>();
    }
}
