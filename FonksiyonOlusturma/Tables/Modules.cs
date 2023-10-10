using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Modules
    {
        public int ModuleId { get; set; }
        public int ProjectId { get; set; }
        public int FuntionId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
    }
}
