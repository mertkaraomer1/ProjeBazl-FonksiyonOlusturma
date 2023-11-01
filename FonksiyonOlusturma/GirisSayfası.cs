﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonksiyonOlusturma
{
    public partial class GirisSayfası : Form
    {
        public GirisSayfası()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            systems sys = new systems();
            sys.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            KullaniciEkranı KE = new KullaniciEkranı();
            KE.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AtananIsler AI = new AtananIsler();
            AI.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UygunOlmayanlar uygunOlmayanlar = new UygunOlmayanlar();
            uygunOlmayanlar.Show();
        }

        private void GirisSayfası_FormClosing(object sender, FormClosingEventArgs e)
        {
           e.Cancel = false;
        }
    }
}
