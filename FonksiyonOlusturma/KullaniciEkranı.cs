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
                        ModuleName = a.ModuleName
                    })
                    .Distinct()
                    .ToList();

                var projectNames = query.Select(a => a.ProjectName).Distinct().ToList();
                var functionNames = query.Select(a => a.FunctionName).Distinct().ToList();
                var moduleNames = query.Select(a => a.ModuleName).Distinct().ToList();

                if (projectNames.Count == 0 || functionNames.Count == 0 || moduleNames.Count == 0)
                {
                    // Eğer veri bulunmuyorsa, comboBox'ları temizle
                    comboBox2.DataSource = null;
                    comboBox3.DataSource = null;
                    comboBox4.DataSource = null;
                }
                else
                {
                    comboBox2.DataSource = projectNames;
                    comboBox3.DataSource = functionNames;
                    comboBox4.DataSource = moduleNames;
                }
            }
            UpdateButtonVisibleState();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string staffName = comboBox1.SelectedItem as string;
            string projectName = comboBox2.SelectedItem as string;
            string functionName = comboBox3.SelectedItem as string;
            string moduleName = comboBox4.SelectedItem as string;

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
            // ComboBox'lardan seçilen değerleri alın
            string staffName = comboBox1.SelectedItem as string;
            string projectName = comboBox2.SelectedItem as string;
            string functionName = comboBox3.SelectedItem as string;
            string moduleName = comboBox4.SelectedItem as string;

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
                    StatusTime = DateTime.Now
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

        private void button3_Click(object sender, EventArgs e)
        {

            // ComboBox'lardan seçilen değerleri alın
            string staffName = comboBox1.SelectedItem as string;
            string projectName = comboBox2.SelectedItem as string;
            string functionName = comboBox3.SelectedItem as string;
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
                string selectedProjectName = comboBox2.SelectedItem?.ToString();
                string selectedFunctionName = comboBox3.SelectedItem?.ToString();
                string selectedModuleName = comboBox4.SelectedItem?.ToString();

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
                    button1.Enabled = false;
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
                string selectedProjectName = comboBox2.SelectedItem?.ToString();
                string selectedFunctionName = comboBox3.SelectedItem?.ToString();
                string selectedModuleName = comboBox4.SelectedItem?.ToString();
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
    }
}
