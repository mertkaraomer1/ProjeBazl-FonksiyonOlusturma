using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm_Fiyat_Güncelleme
{
    public class Fiyat
    {
        [Description("GUİD")]
        public Guid sGuid => Guid.NewGuid();
        [Description("PROJE KODU")]
        public string PROJE_KODU { get; set; }

        [Description("FONKSIYONLAR")]
        public string FONKSIYONLAR { get; set; }



    }
}
