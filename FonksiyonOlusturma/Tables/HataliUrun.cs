﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.Tables
{
    public class HataliUrun
    {
        public int UrunId { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string SiparisNo { get; set; }
        public int HatalıMiktar { get; set; }
        public DateTime Tarih { get; set; }
        public string KayıpZaman { get; set; }
        public string HataTipi { get; set; }
        public string Aciklama { get; set; }
        public string Ozet { get; set; }
        public string HataBolumu { get; set; }
        public string RaporuHazirlayan { get; set; }
        public string HatayıBulanBirim { get; set; }
        public string Resim { get; set; }

    }
}
