using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Assignments
    {
        public int AssignmentId { get; set; }    
        public string ProjectName { get; set; }
        public string FunctionName { get; set; }
        public string ModuleName { get; set; }
        public string StaffName { get; set; }
        public TimeSpan CategoryTime { get; set; } 
        public string CategoryName { get; set; }
    }
}
