using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FonksiyonOlusturma
{
    public partial class Fonksiyonlar : Form
    {
        private MyDbContext dbContext;
        private string? cellValue;
        public Fonksiyonlar()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            // TextBox'ı sadece okunabilir yap
            textBox1.ReadOnly = true;
        }
        public Fonksiyonlar(string? cellValue)
        {
            this.cellValue = cellValue;
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        private void Fonksiyonlar_Load(object sender, EventArgs e)
        {
            FonksiyonGoruntuleme();
        }
        public void FonksiyonGoruntuleme()
        {
            // DataGridView'i temizleyin
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "FONKSİYONLAR");
            dataGridView1.Columns.Add("Column3", "FONKSİYON AÇIKLAMASI");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                string searchText = textBox1.Text; // TextBox1'den gelen veriyi alın

                // TextBox1'deki veriyi Projects tablosundaki ProjectName ile eşleştirin ve ProjectId'leri alın
                var projectIds = dbContext.projects
                    .Where(p => p.ProjectName == searchText)
                    .Select(p => p.ProjectId)
                    .ToList();

                // ProjectId'leri kullanarak Functions tablosundaki FunctionNamelerini ve FunctionDescription'ları alın
                var functionData = dbContext.functions
                    .Where(f => projectIds.Contains(f.ProjectId))
                    .Select(f => new
                    {
                        FunctionName = f.FunctionName,
                        FunctionDescription = f.FunctionDescription
                    })
                    .ToList();

                int satirNo = 1; // Başlangıç satır numarası 1 olarak ayarlanır

                // DataGridView'e satır ekleyin
                foreach (var function in functionData)
                {
                    // DataGridView'e yeni bir satır ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // İlk sütunu (SATIR NO) satır numarası olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // İkinci sütunu (FONKSİYONLAR) fonksiyon adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = function.FunctionName });

                    // Üçüncü sütunu (FONKSİYON AÇIKLAMASI) fonksiyon açıklaması olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = function.FunctionDescription });

                    // DataGridView'e satırı ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her satır ekledikten sonra satır numarasını arttırın
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string selectedProjectName = textBox1.Text; // textbox1'den gelen proje adını alın
            string functionName = textBox2.Text; // TextBox2'den gelen FunctionName'i alın
            string FunctionDescription = textBox3.Text;

            if (!string.IsNullOrEmpty(selectedProjectName) && !string.IsNullOrEmpty(functionName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
                    {
                        // ComboBox1'den seçilen ProjectName ile eşleşen Projects tablosundaki ProjectId'yi bulun
                        int projectId = dbContext.projects
                            .Where(p => p.ProjectName == selectedProjectName)
                            .Select(p => p.ProjectId)
                            .FirstOrDefault();

                        if (projectId != 0) // ProjectId bulunduysa
                        {
                            // Functions tablosuna yeni bir kayıt ekleyin
                            var yeniFonksiyon = new Functions
                            {
                                ProjectId = projectId,
                                FunctionName = functionName,
                                FunctionDescription = FunctionDescription
                            };

                            dbContext.functions.Add(yeniFonksiyon); // Yeni fonksiyonu Functions tablosuna ekleyin
                            dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                            MessageBox.Show("Fonksiyon Kaydedildi...");
                        }
                        else
                        {
                            MessageBox.Show("Proje bulunamadı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Proje adı ve fonksiyon adı girmelisiniz.");
            }
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            FonksiyonGoruntuleme();

        }
        Modul mod;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string textBoxValue = textBox1.Text; // Form2'deki TextBox'tan veriyi al
                Modul mod = new Modul();
                mod.TextBoxValue = textBoxValue; // Form3'teki TextBox'a veriyi aktar

                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[1]; // Tıklanan hücreyi al
                string cellValue = clickedCell.Value.ToString();
                mod.TextBoxValue1 = cellValue;// Form3'teki TextBox2'ye veriyi aktar
                mod.Show();

            }
        }
    }
}
