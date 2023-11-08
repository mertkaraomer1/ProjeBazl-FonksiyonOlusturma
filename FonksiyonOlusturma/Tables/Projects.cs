using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
         public int SystemId { get; set; }
        public Systems System { get; set; } // System ile ilişkilendirme
        public ICollection<Functions> Functions { get; set; }
    }
}
