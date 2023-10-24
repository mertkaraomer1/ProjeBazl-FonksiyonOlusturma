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
            FillComboBox();

        }
        private void FillComboBox()
        {
            // Staffs tablosundan StaffName alanını çekerek ComboBox'ı doldurun
            var staffNames = dbContext.staffs.Select(s => s.StaffName).ToList();

            // ComboBox'a verileri ekleyin
            comboBox1.DataSource = staffNames;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                string selectedStaffName = comboBox1.SelectedItem?.ToString(); // ?. operatörü, null değilse devam etmek için kullanılır.

                var query = dbContext.assignments
                    .Where(a => a.StaffName == selectedStaffName)
                    .Select(a => new
                    {
                        ProjectName = a.ProjectName,
                        FunctionName = a.FunctionName,
                        ModuleName = a.ModuleName,
                        categoryTime = a.CategoryTime,
                    })
                    .Distinct()
                    .ToList();
                dataGridView1.DataSource = query;

            }
            textBox2.Clear();

            textBox3.Clear();

            textBox4.Clear();
            UpdateButtonVisibleState();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string staffName = comboBox1.SelectedItem as string;
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
                    StatusName = "Başla",
                    StatusTime = DateTime.Now
                };

                dbContext.status.Add(newStatus);
                dbContext.SaveChanges();

                MessageBox.Show("Başlandı.");
                UpdateButtonVisibleState();
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
                    "Yorgunluk",
                    "Acil iş",
                    "Öğle yemeği",
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
                    string staffName = comboBox1.SelectedItem as string;
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
                            StatusName = "Araver",
                            StatusTime = DateTime.Now,
                            popup = comboBox.Text
                        };

                        dbContext.status.Add(newStatus);
                        dbContext.SaveChanges();

                        MessageBox.Show("Araverildi.");
                        UpdateButtonVisibleState();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen tüm ComboBox'ları doldurun.");
                    }

                }
                else
                {
                    MessageBox.Show("Lütfen bir ara verme sebebi seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string staffName = comboBox1.SelectedItem as string;
            string projectName = textBox2.Text.ToString();
            string functionName = textBox3.Text.ToString();
            string moduleName = textBox1.Text;

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
                    StatusName = "Bitti",
                    StatusTime = DateTime.Now
                };

                dbContext.status.Add(newStatus);
                dbContext.SaveChanges();

                MessageBox.Show("Bitirildi.");
                UpdateButtonVisibleState();
            }
            else
            {
                MessageBox.Show("Lütfen tüm ComboBox'ları doldurun.");
            }

        }

        private void UpdateButtonVisibleState()
        {
            using (var dbContext = new MyDbContext())
            {
                string staffName = comboBox1.SelectedItem?.ToString();
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateButtonVisibleState();
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
                                .FirstOrDefault(m => m.ModuleName == selectedModuleName && m.FuntionId == functionId && m.ProjectId == projectId);

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır tıklanmış mı kontrolü
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                // Eğer DataGridView'da daha fazla sütununuz varsa, sütun indekslerini düzenleyin
                string kolon1Verisi = selectedRow.Cells["ProjectName"].Value.ToString();
                string kolon2Verisi = selectedRow.Cells["FunctionName"].Value.ToString();
                string kolon3Verisi = selectedRow.Cells["ModuleName"].Value.ToString();

                // TextBox'lara verileri yaz
                textBox2.Text = kolon1Verisi;
                textBox3.Text = kolon2Verisi;
                textBox4.Text = kolon3Verisi;

                // Daha fazla TextBox kontrolünüz varsa, bu şekilde devam edebilirsiniz
            }
        }
    }
}
