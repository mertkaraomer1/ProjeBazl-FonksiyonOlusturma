using FonksiyonOlusturma.MyDb;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FonksiyonOlusturma
{
    public partial class Modul : Form
    {
        private MyDbContext dbContext;
        public Modul()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            // TextBox'ı sadece okunabilir yap
            textBox1.ReadOnly = true;
            // TextBox'ı sadece okunabilir yap
            textBox2.ReadOnly = true;
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        public string TextBoxValue1
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox2.Text = value; }
        }
        private void Modul_Load(object sender, EventArgs e)
        {
            ModuleGoster();
        }
        public void ModuleGoster()
        {
            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "MODÜLLER");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                string searchText1 = textBox1.Text; // TextBox1'den gelen veriyi alın
                string searchText2 = textBox2.Text; // TextBox2'den gelen veriyi alın

                // TextBox1'deki veriyi Projects tablosundaki ProjectName ile eşleştirin ve ProjectId'leri alın
                var projectIds = dbContext.projects
                    .Where(p => p.ProjectName == searchText1)
                    .Select(p => p.ProjectId)
                    .ToList();

                // TextBox2'deki veriyi Functions tablosundaki FunctionName ile eşleştirin ve FunctionId'leri alın
                var functionIds = dbContext.functions
                    .Where(f => f.FunctionName == searchText2)
                    .Select(f => f.FunctionId)
                    .ToList();

                // FunctionId'leri kullanarak Modules tablosundaki ModuleName'leri alın
                var moduleNames = dbContext.modules
                    .Where(m => functionIds.Contains(m.FuntionId))
                    .Select(m => m.ModuleName)
                    .ToList();

                int satirNo = 1; // Başlangıç satır numarası 1 olarak ayarlanır

                // DataGridView'e satır ekleyin
                foreach (var moduleName in moduleNames)
                {
                    // DataGridView'e yeni bir satır ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // İlk sütunu (SATIR NO) satır numarası olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // İkinci sütunu (MODÜLLER) modül adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = moduleName });

                    // DataGridView'e satırı ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her satır ekledikten sonra satır numarasını arttırın
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedFunctionName = textBox2.Text;
            string selectedProjectName = textBox1.Text;
            string modulName = textBox3.Text;
            string ModuleDescription = textBox4.Text;

            if (!string.IsNullOrEmpty(selectedFunctionName) &&
                !string.IsNullOrEmpty(selectedProjectName) &&
                !string.IsNullOrEmpty(modulName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
                    {

                        int functionId = dbContext.functions
                            .Where(f => f.FunctionName == selectedFunctionName)
                            .Select(f => f.FunctionId)
                            .FirstOrDefault();

                        int projectId = dbContext.projects
                            .Where(c => c.ProjectName == selectedProjectName)
                            .Select(c => c.ProjectId)
                            .FirstOrDefault();



                        // Moduls tablosuna yeni bir kayıt ekleyin
                        var yeniModul = new Modules
                        {

                            FuntionId = functionId,
                            ProjectId = projectId,
                            ModuleName = modulName,
                            ModuleDescription = ModuleDescription
                        };

                        dbContext.modules.Add(yeniModul); // Yeni modulu Moduls tablosuna ekleyin
                        dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                        MessageBox.Show("Modul Kaydedildi...");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Tüm alanları doldurmalısınız.");
            }
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            ModuleGoster();
        }
        Atamalar ata;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string textBoxValue = textBox1.Text; // Form2'deki TextBox'tan veriyi al
                string textBoxValue2 = textBox2.Text; // Form2'deki TextBox'tan veriyi al
                Atamalar ata = new Atamalar();
                ata.TextBoxValue = textBoxValue; // Form3'teki TextBox'a veriyi aktar
                ata.TextBoxValue2 = textBoxValue2;
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[1]; // Tıklanan hücreyi al
                string cellValue = clickedCell.Value.ToString();
                ata.TextBoxValue1 = cellValue;// Form3'teki TextBox2'ye veriyi aktar
                ata.Show();

            }
        }
    }
}
