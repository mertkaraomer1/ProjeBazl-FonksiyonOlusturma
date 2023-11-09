using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Records
    {
        public int RecordId { get; set; }
        public string SystemName { get; set; }
        public string ProjectName { get; set; }
        public string FunctionName { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string CategoryName { get; set; }
        public int CategoryTime { get; set; }  
        public string ModuleTip { get; set; }
    }
}
