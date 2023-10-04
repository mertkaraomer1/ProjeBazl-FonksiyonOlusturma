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
            
        }

        private void button1_Click(object sender, EventArgs e)
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
                    StatusName = "Başla",
                    StatusTime = DateTime.Now
                };

                dbContext.status.Add(newStatus);
                dbContext.SaveChanges();

                MessageBox.Show("Başlandı.");
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
            }
            else
            {
                MessageBox.Show("Lütfen tüm ComboBox'ları doldurun.");
            }
        }
    }
}
