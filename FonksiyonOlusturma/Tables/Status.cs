using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Status
    {
        public int StatusId { get; set; }
        public string ModuleName { get; set; }
        public string FunctionName { get; set; }
        public string ProjectName { get; set; }
        public string StaffName { get; set; }
        public TimeSpan CategoryTime { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusTime { get; set; }
        public string popup { get; set; }    
        
    }
}
