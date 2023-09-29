
using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace FonksiyonOlusturma
{
    public partial class Form1 : Form
    {
        private MyDbContext dbContext;
        public Form1()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string projeKodu = textBox1.Text;
            try
            {
                if (!string.IsNullOrEmpty(projeKodu)) // TextBox bo� de�ilse
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
                    {
                        // Projects tablosuna yeni bir proje eklemek i�in a�a��daki gibi bir LINQ sorgusu kullanabilirsiniz:
                        var yeniProje = new Projects
                        {
                            ProjectName = projeKodu
                            // Di�er alanlara da de�er atayabilirsiniz, gerekirse.
                        };

                        dbContext.projects.Add(yeniProje); // Yeni proje nesnesini Projects tablosuna ekleyin
                        dbContext.SaveChanges(); // De�i�iklikleri veritaban�na kaydedin
                    }
                    MessageBox.Show("Proje Kaydedildi...");
                }
                else
                {
                    MessageBox.Show("Proje kodu girmelisiniz."); // TextBox bo�sa kullan�c�ya uyar� verin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata olu�tu: " + ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
            {
                // Projects tablosundaki ProjectName alan�ndaki verileri sorgulay�n
                var projectNames = dbContext.projects.Select(p => p.ProjectName).ToList();

                // ComboBox1 kontrol�ne verileri aktar�n
                comboBox1.DataSource = projectNames;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectedProjectName = comboBox1.SelectedItem as string; // ComboBox1'den se�ilen proje ad�n� al�n
            string functionName = textBox2.Text; // TextBox2'den gelen FunctionName'i al�n

            if (!string.IsNullOrEmpty(selectedProjectName) && !string.IsNullOrEmpty(functionName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
                    {
                        // ComboBox1'den se�ilen ProjectName ile e�le�en Projects tablosundaki ProjectId'yi bulun
                        int projectId = dbContext.projects
                            .Where(p => p.ProjectName == selectedProjectName)
                            .Select(p => p.ProjectId)
                            .FirstOrDefault();

                        if (projectId != 0) // ProjectId bulunduysa
                        {
                            // Functions tablosuna yeni bir kay�t ekleyin
                            var yeniFonksiyon = new Functions
                            {
                                ProjectId = projectId,
                                FunctionName = functionName
                            };

                            dbContext.functions.Add(yeniFonksiyon); // Yeni fonksiyonu Functions tablosuna ekleyin
                            dbContext.SaveChanges(); // De�i�iklikleri veritaban�na kaydedin
                            MessageBox.Show("Fonksiyon Kaydedildi...");
                        }
                        else
                        {
                            MessageBox.Show("Proje bulunamad�.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata olu�tu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Proje ad� ve fonksiyon ad� girmelisiniz.");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
            {

                // Functions tablosundan FunctionName'leri �ekin ve Combobox3'e ekleyin
                var functionNames = dbContext.functions.Select(f => f.FunctionName).ToList();
                comboBox3.DataSource = functionNames;

                // Categories tablosundan CategoryName'leri �ekin ve Combobox4'e ekleyin
                var categoryNames = dbContext.categories.Select(c => c.CategoryName).ToList();
                comboBox4.DataSource = categoryNames;

                // Staffs tablosundan StaffName'leri �ekin ve Combobox5'e ekleyin
                var staffNames = dbContext.staffs.Select(s => s.StaffName).ToList();
                comboBox5.DataSource = staffNames;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string selectedFunctionName = comboBox3.SelectedItem as string;
            string selectedCategoryName = comboBox4.SelectedItem as string;
            string selectedStaffName = comboBox5.SelectedItem as string;
            string modulName = textBox3.Text;

            if (!string.IsNullOrEmpty(selectedFunctionName) &&
                !string.IsNullOrEmpty(selectedCategoryName) &&
                !string.IsNullOrEmpty(selectedStaffName) &&
                !string.IsNullOrEmpty(modulName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
                    {

                        int functionId = dbContext.functions
                            .Where(f => f.FunctionName == selectedFunctionName)
                            .Select(f => f.FunctionId)
                            .FirstOrDefault();

                        int categoryId = dbContext.categories
                            .Where(c => c.CategoryName == selectedCategoryName)
                            .Select(c => c.CategoryID)
                            .FirstOrDefault();

                        int staffId = dbContext.staffs
                            .Where(s => s.StaffName == selectedStaffName)
                            .Select(s => s.StaffId)
                            .FirstOrDefault();

                        // Moduls tablosuna yeni bir kay�t ekleyin
                        var yeniModul = new Modules
                        {

                            FuntionId = functionId,
                            CategoryId = categoryId,
                            StaffId = staffId,
                            ModuleName = modulName
                        };

                        dbContext.modules.Add(yeniModul); // Yeni modulu Moduls tablosuna ekleyin
                        dbContext.SaveChanges(); // De�i�iklikleri veritaban�na kaydedin
                        MessageBox.Show("Modul Kaydedildi...");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata olu�tu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("T�m alanlar� doldurmal�s�n�z.");
            }

        }


    }
}