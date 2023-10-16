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

namespace FonksiyonOlusturma
{
    public partial class Atamalar : Form
    {
        private MyDbContext dbContext;
        public Atamalar()
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
        public void ComboBoxValue1()
        {
            using (var dbContext = new MyDbContext())
            {
                // İlk olarak, textbox1'den gelen ProjectName'i kullanarak ProjectId'yi bulun.
                string projectName = textBox1.Text;
                int projectId = dbContext.projects
                    .Where(p => p.ProjectName == projectName)
                    .Select(p => p.ProjectId)
                    .FirstOrDefault();

                // Ardından, textbox2'den gelen FunctionName'i kullanarak FunctionId'yi bulun.
                string functionName = textBox2.Text;
                int functionId = dbContext.functions
                    .Where(f => f.FunctionName == functionName)
                    .Select(f => f.FunctionId)
                    .FirstOrDefault();

                // Şimdi, ProjectId ve FunctionId'yi kullanarak Modules tablosundan ModuleName'leri alın.
                var moduleNames = dbContext.modules
                    .Where(m => m.ProjectId == projectId && m.FuntionId == functionId)
                    .Select(m => m.ModuleName)
                    .ToList();

                // Elde edilen ModuleName listesini ComboBox3'e atayın.
                comboBox3.DataSource = moduleNames;
            }
        }
        public string TextBoxValue2
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox2.Text = value; }
        }
        public void Yükle()
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                // Staffs tablosundaki StaffName alanındaki verileri sorgulayın
                var staffNames = dbContext.staffs.Select(s => s.StaffName).ToList();
                // ComboBox1 kontrolüne verileri aktarın
                comboBox1.DataSource = staffNames;

                // Categories tablosundaki CategoryName alanındaki verileri sorgulayın
                var categoryNames = dbContext.categories.Select(c => c.CategoryName).ToList();
                // ComboBox2 kontrolüne verileri aktarın
                comboBox2.DataSource = categoryNames;

                // DataGridView'deki verileri sorgulayın ve ProjectName'e göre sıralayın
                var assignments = dbContext.assignments.ToList();
                var projectNameFilter = textBox1.Text; // TextBox1'den gelen değeri alın
                var filteredAssignments = assignments.OrderByDescending(a => a.ProjectName == projectNameFilter).ToList();

                // DataGridView'i güncelleyin veya yeniden doldurun
                dataGridView1.DataSource = filteredAssignments;
            }
            // DataGridView kontrolünüze bir buton sütunu ekleyin.
            DataGridViewImageColumn buttonColumn = new DataGridViewImageColumn();
            buttonColumn.HeaderText = ""; // Sütun başlığı
            buttonColumn.Image = Image.FromFile("delete.png"); // Silme resmini belirtin
            buttonColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Resmi düzgün görüntülemek için ayar
            dataGridView1.Columns.Add(buttonColumn);
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }
        private void Atamalar_Load(object sender, EventArgs e)
        {
            ComboBoxValue1();

            Yükle();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                // TextBox ve ComboBox'lardan gelen verileri alın
                string projectName = textBox1.Text;
                string functionName = textBox2.Text;
                string moduleName = comboBox3.Text;
                string staffName = comboBox1.Text;
                string categoryName = comboBox2.Text;

                // Categories tablosundan CategoryTime'ı alın
                var categoryTime = dbContext.categories
                    .Where(c => c.CategoryName == categoryName)
                    .Select(c => c.CategoryTime)
                    .FirstOrDefault();

                // Assignments tablosuna yeni bir kayıt ekleyin
                Assignments newAssignment = new Assignments
                {
                    ProjectName = projectName,
                    FunctionName = functionName,
                    ModuleName = moduleName,
                    StaffName = staffName,
                    CategoryName = categoryName,
                    CategoryTime = categoryTime // Categories tablosundan alınan CategoryTime
                };

                // Assignment kaydını ekleyin
                dbContext.assignments.Add(newAssignment);
                dbContext.SaveChanges();

                // DataGridView'i güncellemek için sorguyu düzenleyin ve yeniden doldurun
                var assignments = dbContext.assignments.ToList();
                var projectNameFilter = textBox1.Text; // TextBox1'den gelen değeri alın
                var filteredAssignments = assignments.OrderByDescending(a => a.ProjectName == projectNameFilter).ToList();
                dataGridView1.DataSource = filteredAssignments;
            }

        }
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (columnIndex == dataGridView1.Columns[""].Index)
            {
                // İlgili satırda bulunan verilere erişmek için veri modelini kullanabilirsiniz.
                Assignments rowData = new Assignments
                {
                    ProjectName = dataGridView1.Rows[rowIndex].Cells["ProjectName"].Value.ToString(),
                    FunctionName = dataGridView1.Rows[rowIndex].Cells["FunctionName"].Value.ToString(),
                    ModuleName = dataGridView1.Rows[rowIndex].Cells["ModuleName"].Value.ToString(),
                    StaffName = dataGridView1.Rows[rowIndex].Cells["StaffName"].Value.ToString(),
                    CategoryName = dataGridView1.Rows[rowIndex].Cells["CategoryName"].Value.ToString()
                };

                // CategoryTime verisini string olarak aldık, şimdi TimeSpan'e çevirelim
                string categoryTimeStr = dataGridView1.Rows[rowIndex].Cells["CategoryTime"].Value.ToString();

                if (TimeSpan.TryParse(categoryTimeStr, out TimeSpan categoryTime))
                {
                    rowData.CategoryTime = categoryTime;

                    // Entity Framework kullanarak ilgili satırı veritabanından silin
                    using (var dbContext = new MyDbContext())
                    {
                        var assignmentToDelete = dbContext.assignments
                            .FirstOrDefault(a =>
                                a.ProjectName == rowData.ProjectName &&
                                a.FunctionName == rowData.FunctionName &&
                                a.ModuleName == rowData.ModuleName &&
                                a.StaffName == rowData.StaffName &&
                                a.CategoryName == rowData.CategoryName &&
                                a.CategoryTime == rowData.CategoryTime
                            );

                        if (assignmentToDelete != null)
                        {
                            dbContext.assignments.Remove(assignmentToDelete);
                            dbContext.SaveChanges();
                        }
                    }
                    Yükle();
                }
            }


        }
    }
}
