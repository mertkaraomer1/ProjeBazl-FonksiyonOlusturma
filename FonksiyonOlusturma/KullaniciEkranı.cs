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
                    // ModuleName'e bağlı olarak "Bitti" durumunu al
                    var bittiStatus = dbContext.status.FirstOrDefault(s =>
                        s.ProjectName == selectedProjectName &&
                        s.FunctionName == selectedFunctionName &&
                        s.ModuleName == selectedModuleName &&
                        s.StaffName == staffName &&
                        s.StatusName == "Bitti"
                    );

                    if (bittiStatus != null)
                    {
                        // "Bitti" durumunda tüm düğmeler gizlenmelidir
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                    }
                    else
                    {
                        // "Bitti" durumu yoksa, "Araver" durumunu al
                        var araverStatus = dbContext.status.FirstOrDefault(s =>
                            s.ProjectName == selectedProjectName &&
                            s.FunctionName == selectedFunctionName &&
                            s.ModuleName == selectedModuleName &&
                            s.StaffName == staffName &&
                            s.StatusName == "Araver"
                        );

                        if (araverStatus != null)
                        {
                            // "Araver" durumunda button2 ve button3 tıklanamaz
                            button1.Enabled = true; // Button1 etkin
                            button2.Enabled = false;
                            button3.Enabled = false;
                        }
                        else
                        {
                            // "Araver" durumu da yoksa, "Başla" durumunu kabul edin
                            // "Başla" durumunda button1 tıklanamaz
                            button1.Enabled = false;
                            button2.Enabled = true; // Button2 ve Button3 etkin
                            button3.Enabled = true;
                        }
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
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateButtonVisibleState();
        }
    }
}
