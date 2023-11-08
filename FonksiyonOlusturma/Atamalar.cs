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
    public partial class Atamalar : Form
    {
        private MyDbContext dbContext;
        public Atamalar()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }
        public void SistemValue()
        {
            var SistemNames = dbContext.systems
                .Select(p => p.SystemName)
                .ToList();

            // "Sistem Seçiniz" yazısını ekleyin
            SistemNames.Insert(0, "Sistem Seçiniz");

            comboBox1.DataSource = SistemNames;
        }
        public void ProjeValue()
        {
            using (var dbContext = new MyDbContext())
            {
                string SistemName = comboBox1.Text;

                int sistemId = dbContext.systems
                    .Where(p => p.SystemName == SistemName)
                    .Select(p => p.SystemId)
                    .FirstOrDefault();

                var projeNames = dbContext.projects
                    .Where(m => m.SystemId == sistemId)
                    .Select(m => m.ProjectName)
                    .ToList();

                comboBox5.DataSource = projeNames;
            }
        }



        public void Yükle()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("SistemName", "Sistem Number");
            dataGridView1.Columns.Add("ProjeName", "Proje Number");
            dataGridView1.Columns.Add("FunctionName", "Function Number");
            dataGridView1.Columns.Add("ModuleName", "Module Number");
            dataGridView1.Columns.Add("CategoryName", "Category Name");
            dataGridView1.Columns.Add("CategoryTime", "Category Time");
            dataGridView1.Columns.Add("ModuleTip", "ModuleTip");
            string selectedProjectName = comboBox5.Text; // ComboBox5'ten seçilen ProjectName'i alın
            string selectedSystemName=comboBox1.Text;
            // Filter Records tablosunu sadece seçilen ProjectName ile eşleşen kayıtlarla sınırlayın
            var matchingRecords = dbContext.records
                .Where(r => r.ProjectName == selectedProjectName)
                .ToList();

            // Mevcut satırları DataGridView'den temizleyin
            dataGridView1.Rows.Clear();

            // Sadece seçilen ProjectName ile eşleşen Records kayıtlarını işleyin
            foreach (var record in matchingRecords)
            {
                // Eşleşen bir Assignment kaydı bulmak için sorgu yapın
                var matchingAssignment = dbContext.assignments
                    .FirstOrDefault(a => a.ProjectName == selectedProjectName &&
                                         a.SystemName == selectedSystemName &&
                                         a.FunctionName == record.FunctionName &&
                                         a.ModuleName == record.ModuleName &&
                                         a.CategoryName == record.CategoryName &&
                                         a.CategoryTime == record.CategoryTime&&
                                         a.ModuleTip==record.ModuleTip);

                // Eşleşen bir Assignment kaydı bulunmazsa, DataGridView'e ekleyin
                if (matchingAssignment == null)
                {
                    dataGridView1.Rows.Add(
                        record.SystemName,
                        record.ProjectName,
                        record.FunctionName,
                        record.ModuleName,
                        record.CategoryName,
                        record.CategoryTime,
                        record.ModuleTip
                    );
                }
            }


            var staffNames = dbContext.staffs.Select(c=>c.StaffName).ToList();

            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.Name = "StaffNameColumn";
            comboBoxColumn.HeaderText = "Staff Name";
            comboBoxColumn.Items.AddRange(staffNames.ToArray());
            dataGridView1.Columns.Add(comboBoxColumn);
        }

        private void Atamalar_Load(object sender, EventArgs e)
        {
            Atanmıslar();
            Grafik();
            SistemValue();
            Yükle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Assignments> assignments = new List<Assignments>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null && row.Cells[4].Value != null && row.Cells[5].Value != null && row.Cells[6].Value != null)
                {
                    int categoryTime;
                    if (int.TryParse(row.Cells[5].Value.ToString(), out categoryTime))
                    {
                        Assignments assignment = new Assignments
                        {
                            SystemName = row.Cells[0].Value.ToString(),
                            ProjectName = row.Cells[1].Value.ToString(),
                            FunctionName = row.Cells[2].Value.ToString(),
                            ModuleName = row.Cells[3].Value.ToString(),
                            StaffName = row.Cells[7].Value.ToString(),
                            CategoryTime = categoryTime,
                            CategoryName = row.Cells[4].Value.ToString(),
                            Status = "True",
                            ModuleTip = row.Cells[6].Value.ToString()
                            
                        };
                        assignments.Add(assignment);
                    }
                    else
                    {
                        // Geçerli bir TimeSpan formatına dönüşüm yapılamadı, hata işleme veya bildirim ekleme
                    }
                }
            }



            using (var dbContext = new MyDbContext())
            {
                foreach (Assignments assignment in assignments)
                {
                    dbContext.assignments.Add(assignment);
                }

                dbContext.SaveChanges();
            }
            Yükle();
            Atanmıslar();
            Grafik();

        }
        public void Atanmıslar()
        {
            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();
            // DataGridView sütunlarını oluşturun
            dataGridView3.Columns.Add("SystemName", "Sistem Number");
            dataGridView3.Columns.Add("ProjectName", "Proje Number");
            dataGridView3.Columns.Add("FunctionName", "Function Number");
            dataGridView3.Columns.Add("ModuleName", "Module Number");
            dataGridView3.Columns.Add("StaffName", "Staff Name");
            dataGridView3.Columns.Add("CategoryName", "Category Name");
            dataGridView3.Columns.Add("CategoryTime", "Category Time");
            dataGridView3.Columns.Add("ModuleTip", "Module Tip");
            // DataGridView kontrolünüze bir buton sütunu ekleyin.
            DataGridViewImageColumn buttonColumn = new DataGridViewImageColumn();
            buttonColumn.HeaderText = "SİL"; // Sütun başlığı
            buttonColumn.Image = Image.FromFile("delete.png"); // Silme resmini belirtin
            buttonColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Resmi düzgün görüntülemek için ayar
            dataGridView3.Columns.Add(buttonColumn);

            // ComboBox5'ten seçilen değeri al
            string selectedProjectName = comboBox5.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedProjectName))
            {
                using (var dbContext = new MyDbContext())
                {
                    // ComboBox5'ten gelen ProjectName ile eşleşen ve FinishTime'ı null olan kayıtları al
                    var filteredAssignments = dbContext.assignments
                        .Where(a => a.ProjectName == selectedProjectName && a.Status == "True")
                        .Select(a => new
                        {
                            a.SystemName,
                            a.ProjectName,
                            a.FunctionName,
                            a.ModuleName,
                            a.StaffName,
                            a.CategoryName,
                            a.CategoryTime,
                            a.ModuleTip
                        })
                        .ToList();

                    // Verileri DataGridView'e ekleyin
                    foreach (var assignment in filteredAssignments)
                    {
                        dataGridView3.Rows.Add(
                            assignment.SystemName,
                            assignment.ProjectName,
                            assignment.FunctionName,
                            assignment.ModuleName,
                            assignment.StaffName,
                            assignment.CategoryName,
                            assignment.CategoryTime,
                            assignment.ModuleTip

                        );
                    }
                }
            }


        }

        public void Grafik()
        {
            dataGridView2.Columns.Add("Column0", "Staff Name");
            dataGridView2.Columns.Add("Column1", "Total time");

            // StaffNames verilerini al
            var staffNames = dbContext.staffs.Select(s => s.StaffName).ToList();

            // DataGridView'i temizle
            dataGridView2.Rows.Clear();

            foreach (var staffName in staffNames)
            {
                var assignmentsForStaff = dbContext.assignments
                    .Where(a => a.StaffName == staffName && a.Status == "True") // FinishTime'ı null olanları filtrele
                    .ToList()
                    .Select(a => new
                    {
                        StaffName = a.StaffName,
                        CategoryTime = a.CategoryTime
                    })
                    .ToList();

                int totalCategoryTime = 0;

                foreach (var assignment in assignmentsForStaff)
                {
                    totalCategoryTime += assignment.CategoryTime;
                }

                // DataGridView'e ekle
                dataGridView2.Rows.Add(staffName, totalCategoryTime);
            }




        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
            Yükle();
            Atanmıslar();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjeValue();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 7) // 7. sütundaki düğme tıklandı mı kontrol ediliyor
            {
                // Seçilen satırın verilerini almak için DataGridView'den erişin
                DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];

                // Verileri değişkenlere aktarın
                string staffName = selectedRow.Cells["StaffName"].Value.ToString();
                string projectName = selectedRow.Cells["ProjectName"].Value.ToString();
                string functionName = selectedRow.Cells["FunctionName"].Value.ToString();
                string moduleName = selectedRow.Cells["ModuleName"].Value.ToString();

                // Değişkenlere sahip görevi `Assignments` tablosunda arayın
                using (var dbContext = new MyDbContext())
                {
                    var assignmentToRemove = dbContext.assignments.FirstOrDefault(a =>
                        a.StaffName == staffName &&
                        a.ProjectName == projectName &&
                        a.FunctionName == functionName &&
                        a.ModuleName == moduleName);

                    if (assignmentToRemove != null)
                    {
                        // Görevi veritabanından kaldırın
                        dbContext.assignments.Remove(assignmentToRemove);
                        dbContext.SaveChanges();

                        // Veri kaynağınızı güncelleyin (örneğin, verileri tekrar yükleyin veya veri kaynağınızı güncelleyin)
                        // Bu, DataGridView'nin otomatik olarak güncellenmesini sağlar
                        // Örnek olarak, Yükle() işlevini çağırabilirsiniz:
                        Atanmıslar();
                    }
                }
            }

        }
    }
}
