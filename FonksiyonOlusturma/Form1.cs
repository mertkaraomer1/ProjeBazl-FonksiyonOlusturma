
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
                if (!string.IsNullOrEmpty(projeKodu)) // TextBox boþ deðilse
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
                    {
                        // Projects tablosuna yeni bir proje eklemek için aþaðýdaki gibi bir LINQ sorgusu kullanabilirsiniz:
                        var yeniProje = new Projects
                        {
                            ProjectName = projeKodu
                            // Diðer alanlara da deðer atayabilirsiniz, gerekirse.
                        };

                        dbContext.projects.Add(yeniProje); // Yeni proje nesnesini Projects tablosuna ekleyin
                        dbContext.SaveChanges(); // Deðiþiklikleri veritabanýna kaydedin
                    }
                    MessageBox.Show("Proje Kaydedildi...");
                }
                else
                {
                    MessageBox.Show("Proje kodu girmelisiniz."); // TextBox boþsa kullanýcýya uyarý verin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluþtu: " + ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
            {
                // Projects tablosundaki ProjectName alanýndaki verileri sorgulayýn
                var projectNames = dbContext.projects.Select(p => p.ProjectName).ToList();

                // ComboBox1 kontrolüne verileri aktarýn
                comboBox1.DataSource = projectNames;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectedProjectName = comboBox1.SelectedItem as string; // ComboBox1'den seçilen proje adýný alýn
            string functionName = textBox2.Text; // TextBox2'den gelen FunctionName'i alýn

            if (!string.IsNullOrEmpty(selectedProjectName) && !string.IsNullOrEmpty(functionName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
                    {
                        // ComboBox1'den seçilen ProjectName ile eþleþen Projects tablosundaki ProjectId'yi bulun
                        int projectId = dbContext.projects
                            .Where(p => p.ProjectName == selectedProjectName)
                            .Select(p => p.ProjectId)
                            .FirstOrDefault();

                        if (projectId != 0) // ProjectId bulunduysa
                        {
                            // Functions tablosuna yeni bir kayýt ekleyin
                            var yeniFonksiyon = new Functions
                            {
                                ProjectId = projectId,
                                FunctionName = functionName
                            };

                            dbContext.functions.Add(yeniFonksiyon); // Yeni fonksiyonu Functions tablosuna ekleyin
                            dbContext.SaveChanges(); // Deðiþiklikleri veritabanýna kaydedin
                            MessageBox.Show("Fonksiyon Kaydedildi...");
                        }
                        else
                        {
                            MessageBox.Show("Proje bulunamadý.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Proje adý ve fonksiyon adý girmelisiniz.");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
            {

                // Functions tablosundan FunctionName'leri çekin ve Combobox3'e ekleyin
                var functionNames = dbContext.functions.Select(f => f.FunctionName).ToList();
                comboBox3.DataSource = functionNames;

                // Categories tablosundan CategoryName'leri çekin ve Combobox4'e ekleyin
                var categoryNames = dbContext.categories.Select(c => c.CategoryName).ToList();
                comboBox4.DataSource = categoryNames;

                // Staffs tablosundan StaffName'leri çekin ve Combobox5'e ekleyin
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
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
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

                        // Moduls tablosuna yeni bir kayýt ekleyin
                        var yeniModul = new Modules
                        {

                            FuntionId = functionId,
                            CategoryId = categoryId,
                            StaffId = staffId,
                            ModuleName = modulName
                        };

                        dbContext.modules.Add(yeniModul); // Yeni modulu Moduls tablosuna ekleyin
                        dbContext.SaveChanges(); // Deðiþiklikleri veritabanýna kaydedin
                        MessageBox.Show("Modul Kaydedildi...");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Tüm alanlarý doldurmalýsýnýz.");
            }

        }


    }
}