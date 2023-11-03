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

namespace FonksiyonOlusturma
{
    public partial class KullaniciEkranı : Form
    {
        private MyDbContext dbContext;
        public KullaniciEkranı()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            yükle();

        }
        int kolon4Verisi;

        DataTable table = new DataTable();
        public void yükle()
        {
            advancedDataGridView1.Columns.Clear();
            table.Rows.Clear();
            table.Columns.Clear();
            if (table.Columns.Count == 0)
            {
                table.Columns.Add("System Number");
                table.Columns.Add("Project Number");
                table.Columns.Add("Function Number");
                table.Columns.Add("Module Number");
                table.Columns.Add("Category Name");
                table.Columns.Add("Category Time");
                table.Columns.Add("Staff Name");
                table.Columns.Add("Module Tip");
                table.Columns.Add("Status");
                table.Columns.Add("Ara Süresi");
            }

            var query = dbContext.assignments
                .Where(a => a.Status != "False")
                .Select(a => new
                {
                    SystemName = a.SystemName,
                    ProjectName = a.ProjectName,
                    FunctionName = a.FunctionName,
                    ModuleName = a.ModuleName,
                    ModuleTip = a.ModuleTip,
                    CategoryTime = a.CategoryTime,
                    StaffName = a.StaffName,
                    CategoryName = a.CategoryName,
                    Status = a.Status,
                })
                .ToList();

            if (query.Any())
            {
                foreach (var item in query)
                {
                    var latestStatus = dbContext.status
                        .Where(a => a.ProjectName == item.ProjectName &&
                                    a.FunctionName == item.FunctionName &&
                                    a.ModuleName == item.ModuleName &&
                                    a.ModuleTip == item.ModuleTip)
                        .Select(a => new
                        {
                            statusName = a.StatusName,
                            statusTime = a.StatusTime
                        })
                        .OrderByDescending(a => a.statusTime)
                        .FirstOrDefault();

                    var latestAraVerStatus = dbContext.status
                        .Where(a => a.ProjectName == item.ProjectName &&
                                    a.FunctionName == item.FunctionName &&
                                    a.ModuleName == item.ModuleName &&
                                    a.ModuleTip == item.ModuleTip &&
                                    a.StatusName == "Araver")
                        .Select(a => new
                        {
                            statusTime = a.StatusTime
                        })
                        .OrderByDescending(a => a.statusTime)
                        .FirstOrDefault();

                    if (latestStatus != null)
                    {
                        // Veri bulunduğunda en son statusun zamanını al
                        DateTime latestStatusTime = latestStatus.statusTime;

                        if (latestAraVerStatus != null)
                        {
                            // Araver'deki zamanı al
                            DateTime araverTime = latestAraVerStatus.statusTime;

                            // Farkı hesapla
                            TimeSpan timeDifference = latestStatusTime - araverTime;

                            // Farkı datagridview'e yazdır
                            table.Rows.Add(
                                item.SystemName,
                                item.ProjectName,
                                item.FunctionName,
                                item.ModuleName,
                                item.CategoryName,
                                item.CategoryTime,
                                item.StaffName,
                                item.ModuleTip,
                                latestStatus.statusName,
                               timeDifference.TotalMinutes.ToString("0.00") + " dakika"
                            );
                        }
                        else
                        {
                            // Araver verisi bulunamadığında "Bekliyor" yazdır
                            table.Rows.Add(
                                item.SystemName,
                                item.ProjectName,
                                item.FunctionName,
                                item.ModuleName,
                                item.CategoryName,
                                item.CategoryTime,
                                item.StaffName,
                                item.ModuleTip,
                                latestStatus.statusName,
                                "Ara Verilmedi..."
                            );
                        }
                    }
                    else
                    {
                        // Veri bulunamadığında "Devam Ediyor" yazdır
                        table.Rows.Add(
                            item.SystemName,
                            item.ProjectName,
                            item.FunctionName,
                            item.ModuleName,
                            item.CategoryName,
                            item.CategoryTime,
                            item.StaffName,
                            item.ModuleTip,
                            "Bekliyor...",
                            "Bekliyor..."
                        );
                    }
                }
            }






            advancedDataGridView1.DataSource = table;


            textBox2.Clear();

            textBox3.Clear();

            textBox4.Clear();
            UpdateButtonVisibleState();
        }


        string staffname;
        string moduleTip;
        private void button1_Click(object sender, EventArgs e)
        {


            string projectName = textBox2.Text.ToString();
            string functionName = textBox3.Text.ToString();
            string moduleName = textBox4.Text.ToString();

            if (!string.IsNullOrEmpty(staffname) &&
                !string.IsNullOrEmpty(projectName) &&
                !string.IsNullOrEmpty(functionName) &&
                !string.IsNullOrEmpty(moduleName))
            {
                // Status tablosuna yeni bir kayıt ekleyin
                Status newStatus = new Status
                {
                    ModuleName = moduleName,
                    FunctionName = functionName,
                    ProjectName = projectName,
                    StaffName = staffname,
                    CategoryTime = kolon4Verisi,
                    StatusName = "Başla",
                    StatusTime = DateTime.Now,
                    ModuleTip = moduleTip

                };

                dbContext.status.Add(newStatus);
                dbContext.SaveChanges();

                MessageBox.Show("Başlandı.");
                UpdateButtonVisibleState();
                yükle();
            }
            else
            {
                MessageBox.Show("Lütfen tüm ComboBox'ları doldurun.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form sebepSecimiForm = new Form
            {
                Text = "Ara Verme Sebebinizi Seçiniz...",
                Size = new Size(300, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            ComboBox comboBox = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            comboBox.Items.AddRange(new string[]
            {
                    "Mesai Bitimi",
                    "İş değişimi(Tanımlı)",
                    "Eğitim",
                    "Toplantı",
                    "Diğer"
            });

            Button onayButton = new Button
            {
                Text = "Onay",
                Location = new Point(100, 60),
                Size = new Size(60, 30),
            };

            onayButton.Click += (s, e) =>
            {
                string selectedReason = comboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedReason))
                {
                    MessageBox.Show($"Ara verme sebebi: {selectedReason}\nAra vermek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo);
                    // ComboBox'lardan seçilen değerleri alın
                    string staffName = staffname;
                    string projectName = textBox2.Text.ToString();
                    string functionName = textBox3.Text.ToString();
                    string moduleName = textBox4.Text.ToString();


                    if (!string.IsNullOrEmpty(staffName) &&
                        !string.IsNullOrEmpty(projectName) &&
                        !string.IsNullOrEmpty(functionName) &&
                        !string.IsNullOrEmpty(moduleName))
                    {
                        // Status tablosuna yeni bir kayıt ekleyin
                        Status newStatus = new Status
                        {
                            StaffName = staffName,
                            ProjectName = projectName,
                            FunctionName = functionName,
                            ModuleName = moduleName,
                            CategoryTime = kolon4Verisi,
                            StatusName = "Araver",
                            StatusTime = DateTime.Now,
                            popup = comboBox.Text,
                            ModuleTip = moduleTip
                        };

                        dbContext.status.Add(newStatus);
                        dbContext.SaveChanges();

                        MessageBox.Show("Araverildi.");
                        UpdateButtonVisibleState();
                        yükle();
                    }

                }


                sebepSecimiForm.Close();
            };

            sebepSecimiForm.Controls.Add(comboBox);
            sebepSecimiForm.Controls.Add(onayButton);

            sebepSecimiForm.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {

            // ComboBox'lardan seçilen değerleri alın
            string staffName = staffname;
            string projectName = textBox2.Text;
            string functionName = textBox3.Text;
            string moduleName = textBox4.Text;

            if (!string.IsNullOrEmpty(staffName) &&
                !string.IsNullOrEmpty(projectName) &&
                !string.IsNullOrEmpty(functionName) &&
                !string.IsNullOrEmpty(moduleName))
            {
                using (var dbContext = new MyDbContext())
                {
                    // Status tablosuna yeni bir kayıt ekleyin
                    Status newStatus = new Status
                    {
                        StaffName = staffName,
                        ProjectName = projectName,
                        FunctionName = functionName,
                        ModuleName = moduleName,
                        CategoryTime = kolon4Verisi,
                        StatusName = "Bitti",
                        StatusTime = DateTime.Now,
                        ModuleTip = moduleTip
                    };

                    dbContext.status.Add(newStatus);
                    dbContext.SaveChanges();

                    MessageBox.Show("Bitirildi.");
                    UpdateButtonVisibleState();
                }

                using (var dbContext = new MyDbContext())
                {
                    var assignmentToUpdate = dbContext.assignments
                        .FirstOrDefault(a => a.StaffName == staffName &&
                                             a.ProjectName == projectName &&
                                             a.FunctionName == functionName &&
                                             a.ModuleName == moduleName);
                    if (assignmentToUpdate != null)
                    {

                        //  Status alanlarını güncelle
                        assignmentToUpdate.Status = "False";

                        // Null değerleri atamak için NullReferenceException hatasını önlemek için kontrol eklemeye gerek yok
                        dbContext.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm gerekli alanları doldurun.");
            }
            yükle();
        }

        private void UpdateButtonVisibleState()
        {
            using (var dbContext = new MyDbContext())
            {
                string staffName = staffname;
                string selectedProjectName = textBox2.Text.ToString();
                string selectedFunctionName = textBox3.Text.ToString();
                string selectedModuleName = textBox4.Text.ToString();

                if (staffName != null && selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Seçilen verilere göre Status tablosundaki en son StatusTime'ı al
                    var lastStatusTime = dbContext.status
                        .Where(s =>
                            s.ProjectName == selectedProjectName &&
                            s.FunctionName == selectedFunctionName &&
                            s.ModuleName == selectedModuleName &&
                            s.StaffName == staffName)
                        .OrderByDescending(s => s.StatusTime)
                        .Select(s => s.StatusTime)
                        .FirstOrDefault();

                    if (lastStatusTime != null)
                    {
                        // En son StatusTime'a göre StatusName'i al
                        var lastStatus = dbContext.status
                            .Where(s =>
                                s.ProjectName == selectedProjectName &&
                                s.FunctionName == selectedFunctionName &&
                                s.ModuleName == selectedModuleName &&
                                s.StaffName == staffName &&
                                s.StatusTime == lastStatusTime)
                            .Select(s => s.StatusName)
                            .FirstOrDefault();

                        // StatusName'e göre düğme durumlarını ayarla
                        if (lastStatus == "Bitti")
                        {
                            // "Bitti" durumunda tüm düğmeler gizlenmelidir
                            button1.Enabled = false;
                            button2.Enabled = false;
                            button3.Enabled = false;
                        }
                        else if (lastStatus == "Araver")
                        {
                            // "Araver" durumunda button2 ve button3 tıklanamaz
                            button1.Enabled = true; // Button1 etkin
                            button2.Enabled = false;
                            button3.Enabled = false;
                        }
                        else if (lastStatus == "Başla")
                        {
                            // "Başla" durumunda button1 tıklanamaz
                            button1.Enabled = false;
                            button2.Enabled = true; // Button2 ve Button3 etkin
                            button3.Enabled = true;
                        }
                    }
                    else
                    {
                        // Veri bulunamazsa tüm düğmeler etkin değil
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                    }
                }
                else
                {
                    // ComboBox'lar null ise, tüm düğmeler etkin değil
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                }
            }



        }

        private void KullaniciEkranı_Load(object sender, EventArgs e)
        {
            UpdateButtonVisibleState();
            groupBox2.Visible = false;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                string selectedProjectName = textBox2.Text.ToString();
                string selectedFunctionName = textBox3.Text.ToString();
                string selectedModuleName = textBox4.Text.ToString();
                string newModuleName = textBox1.Text;
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Seçilen ProjectName'e göre ProjectId'yi alın
                    int projectId = dbContext.projects
                        .Where(p => p.ProjectName == selectedProjectName)
                        .Select(p => p.ProjectId)
                        .FirstOrDefault();

                    if (projectId > 0)
                    {
                        // Seçilen FunctionName'e göre FunctionId'yi alın
                        int functionId = dbContext.functions
                            .Where(f => f.FunctionName == selectedFunctionName)
                            .Select(f => f.FunctionId)
                            .FirstOrDefault();

                        if (functionId > 0)
                        {
                            // Seçilen ModuleName'i güncellemek için ilgili Module kaydını bulun
                            var module = dbContext.modules
                                .FirstOrDefault(m => m.ModuleName == selectedModuleName && m.FunctionId == functionId && m.ProjectId == projectId);

                            if (module != null)
                            {
                                // TextBox1'den gelen veri ile ModuleName'i güncelleyin
                                module.ModuleName = textBox1.Text;

                                // Değişiklikleri veritabanına kaydedin
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Assignments tablosunda seçilen verilere göre ModuleName'i güncelle
                    var assignmentsToUpdate = dbContext.assignments
                        .Where(a => a.ProjectName == selectedProjectName &&
                                    a.FunctionName == selectedFunctionName &&
                                    a.ModuleName == selectedModuleName);

                    foreach (var assignment in assignmentsToUpdate)
                    {
                        assignment.ModuleName = textBox1.Text;
                    }

                    // Değişiklikleri veritabanına kaydet
                    dbContext.SaveChanges();
                }
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Aynı ProjectName ve FunctionName'e sahip tüm ModuleName'leri al
                    var modulesToUpdate = dbContext.status
                        .Where(s => s.ProjectName == selectedProjectName &&
                                    s.FunctionName == selectedFunctionName)
                        .ToList();

                    foreach (var status in modulesToUpdate)
                    {
                        status.ModuleName = newModuleName;
                    }

                    // Değişiklikleri veritabanına kaydet
                    dbContext.SaveChanges();

                    MessageBox.Show("Değişiklikler Kaydedildi.");
                }


            }
        }

        private void advancedDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır tıklanmış mı kontrolü
            {
                DataGridViewRow selectedRow = advancedDataGridView1.Rows[e.RowIndex];
                // Eğer DataGridView'da daha fazla sütununuz varsa, sütun indekslerini düzenleyin
                string kolon1Verisi = selectedRow.Cells["Project Number"].Value.ToString();
                string kolon2Verisi = selectedRow.Cells["Function Number"].Value.ToString();
                string kolon3Verisi = selectedRow.Cells["Module Number"].Value.ToString();
                kolon4Verisi = Convert.ToInt32(selectedRow.Cells["Category Time"].Value.ToString());
                staffname = selectedRow.Cells["Staff Name"].Value.ToString();
                moduleTip = selectedRow.Cells["Module Tip"].Value.ToString();

                // TextBox'lara verileri yaz
                textBox2.Text = kolon1Verisi;
                textBox3.Text = kolon2Verisi;
                textBox4.Text = kolon3Verisi;
                UpdateButtonVisibleState();
            }
        }
    }
}
