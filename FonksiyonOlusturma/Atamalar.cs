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
            // TextBox'ı sadece okunabilir yap
            textBox3.ReadOnly = true;
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        public string TextBoxValue1
        {
            get { return textBox3.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox3.Text = value; }
        }
        public string TextBoxValue2
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox2.Text = value; }
        }
        private void Atamalar_Load(object sender, EventArgs e)
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
                // DataGridView'i güncelleyin veya yeniden doldurun
                dataGridView1.DataSource = dbContext.assignments.ToList();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                // TextBox ve ComboBox'lardan gelen verileri alın
                string projectName = textBox1.Text;
                string functionName = textBox2.Text;
                string moduleName = textBox3.Text;
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

                // DataGridView'i güncelleyin veya yeniden doldurun
                dataGridView1.DataSource = dbContext.assignments.ToList();
            }

        }


    }
}
