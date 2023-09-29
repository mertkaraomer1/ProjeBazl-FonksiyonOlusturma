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
            // ComboBox'ları temizle
            comboBox2.DataSource = null;
            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            // ComboBox'ta seçilen StaffName'i alın
            string selectedStaffName = comboBox1.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedStaffName))
            {
                // Seçilen StaffName'e göre StaffId'yi bulun
                int staffId = dbContext.staffs
                    .Where(s => s.StaffName == selectedStaffName)
                    .Select(s => s.StaffId)
                    .FirstOrDefault();

                // Modules tablosunda StaffId ile eşleşen kayıtları alın
                var modules = dbContext.modules
                    .Where(m => m.StaffId == staffId)
                    .ToList();

                // ModuleName'leri ComboBox4'e ekleyin
                comboBox4.DataSource = modules.Select(m => m.ModuleName).ToList();

                // FonksiyonId'leri listelere ekleyin
                var functionIds = modules.Select(m => m.FuntionId).ToList();

                // FunctionId'leri kullanarak FunctionName'leri alın
                var functionNames = dbContext.functions
                    .Where(f => functionIds.Contains(f.FunctionId))
                    .Select(f => f.FunctionName)
                    .ToList();

                // FunctionName'leri ComboBox3'e ekleyin
                comboBox3.DataSource = functionNames;

                // ProjeId'leri Functions tablosundan alın
                var projectIds = dbContext.functions
                    .Where(f => functionIds.Contains(f.FunctionId))
                    .Select(f => f.ProjectId)
                    .ToList();

                // ProjeId'leri kullanarak ProjectName'i alın
                var projectNames = dbContext.projects
                    .Where(p => projectIds.Contains(p.ProjectId))
                    .Select(p => p.ProjectName)
                    .ToList();

                // ProjectName'i ComboBox2'ye ekleyin
                comboBox2.DataSource = projectNames;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
