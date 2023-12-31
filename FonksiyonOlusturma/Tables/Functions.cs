﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Functions
    {
        public int FunctionId { get; set; }
        public int ProjectId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionDescription { get; set; }
        public Projects Project { get; set; } // Project ile ilişkilendirme
        public ICollection<Modules> Modules { get; set; }
    }
}
