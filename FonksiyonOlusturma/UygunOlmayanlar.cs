﻿using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System;
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
    public partial class UygunOlmayanlar : Form
    {
        private MyDbContext dbContext;
        public UygunOlmayanlar()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Veritabanı bağlamınızı oluşturun
            using (var dbContext = new MyDbContext())
            {
                // 2. HataliUrun sınıfı ile eşleşen bir nesne oluşturun ve verileri atayın
                var yeniHataliUrun = new HataliUrun
                {
                    UrunKodu = textBox1.Text,
                    UrunAdi = textBox2.Text,
                    SiparisNo = textBox3.Text,
                    HatalıMiktar = Convert.ToInt32(textBox4.Text),
                    ToplamMiktar = Convert.ToInt32(textBox5.Text),
                    Tarih = DateTime.Now, // Tarih alanı için uygun bir değer atayın
                    KayıpZaman = textBox7.Text,
                    HataTipi = textBox8.Text,
                    Aciklama = textBox9.Text,
                    HataBolumu = textBox10.Text,
                    RaporuHazirlayan = textBox11.Text,
                    Resim = textBox6.Text
                };

                // 3. Veritabanına ekleme işlemi
                dbContext.hataliUruns.Add(yeniHataliUrun);
                dbContext.SaveChanges(); // Değişiklikleri kaydedin
                MessageBox.Show("Kaydedildi...");
                Listele();
            }
        }
        public void Listele()
        {
            using (var context = new MyDbContext())
            {
                // Veritabanından HataliUrun tablosundaki verileri çekin
                List<HataliUrun> hataliUrunList = context.hataliUruns.ToList();

                // DataGridView'e BindingList olarak verileri bağlayın
                BindingList<HataliUrun> bindingList = new BindingList<HataliUrun>(hataliUrunList);
                dataGridView1.DataSource = bindingList;
            }

        }

        private void UygunOlmayanlar_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox6.Text = openFileDialog1.FileName;
        }

        Resim resim;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                Resim resim = new Resim();

                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[12]; // Tıklanan hücreyi al
                string cellValue = clickedCell.Value.ToString();
                resim.picturebox = cellValue;// Form3'teki TextBox2'ye veriyi aktar
                resim.Show();

            }
        }
    }
}
