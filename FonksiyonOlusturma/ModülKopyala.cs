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
using Zuby.ADGV;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace FonksiyonOlusturma
{
    public partial class ModülKopyala : Form
    {
        private MyDbContext dbContext;
        public ModülKopyala()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            // TextBox'ı sadece okunabilir yap
            textBox1.ReadOnly = true;
            // TextBox'ı sadece okunabilir yap
            textBox2.ReadOnly = true;
        }
        public string TextBoxValueProje
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox2.Text = value; }
        }
        public string TextBoxValueFonksiyon
        {
            get { return textBox3.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox3.Text = value; }
        }
        public string TextBoxValueSistem
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        public void proje()
        {
            var projectCodes = dbContext.projects
                .Select(p => p.ProjectName)
                .ToList();

            comboBox1.DataSource = projectCodes;

        }
        public void fonksiyon()
        {
            var projectcode = comboBox1.SelectedItem;
            var projectCodes = dbContext.projects.Where(x => x.ProjectName == projectcode)
                .Select(p => p.ProjectId)
                .ToList();

            var functionData = dbContext.functions
                .Where(f => projectCodes.Contains(f.ProjectId))
                .Select(f => f.FunctionName)
                .ToList();

            // "Fonksiyon Seçiniz" seçeneğini ekle
            functionData.Insert(0, "Fonksiyon Seçiniz");

            // ComboBox'a veriyi ata
            comboBox2.DataSource = functionData;

        }
        public void yükle()
        {
            // Veriyi temizle
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // İlk sütuna checkbox ekle
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Name = "CheckBoxColumn";
            checkBoxColumn.FalseValue = false;
            checkBoxColumn.TrueValue = true;
            checkBoxColumn.Width = 30;
            dataGridView1.Columns.Add(checkBoxColumn);
            dataGridView1.Columns.Add("ModuleName", "Module Name");
            dataGridView1.Columns.Add("ModuleDescription", "Module Description");
            dataGridView1.Columns.Add("ModuleTip", "Module Tip");
            dataGridView1.Columns.Add("Description", "Description");

            var functionData = comboBox2.SelectedItem;
            var projectcode = comboBox1.SelectedItem;

            var projectCodes = dbContext.projects.Where(x => x.ProjectName == projectcode)
                .Select(p => p.ProjectId)
                .ToList();

            var FunctionCodes = dbContext.functions.Where(x => x.FunctionName == functionData)
                .Select(p => p.FunctionId)
                .ToList();

            var ModuleCode = dbContext.modules.Where(x => projectCodes.Contains(x.ProjectId) && FunctionCodes.Contains(x.FunctionId))
                .Select(x => new
                {
                    x.ModuleName,
                    x.ModuleDescription,
                    x.ModuleTip,
                    x.Description,
                }).ToList();

            foreach (var module in ModuleCode)
            {
                // Her satır için varsayılan checkbox değerini ata
                dataGridView1.Rows.Add(false, module.ModuleName, module.ModuleDescription, module.ModuleTip, module.Description);
            }

            var Category = dbContext.categories.Select(x => x.CategoryName);
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.Name = "CategoryColumn";
            comboBoxColumn.HeaderText = "Category";
            comboBoxColumn.Items.AddRange(Category.ToArray());
            dataGridView1.Columns.Add(comboBoxColumn);
        }


        private void ModülKopyala_Load(object sender, EventArgs e)
        {
            proje();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fonksiyon();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            yükle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SistemAdi = textBox1.Text;
            string projeadi = textBox2.Text;
            string fonksiyonAdi = textBox3.Text;

            var projeID = dbContext.projects.Where(x => x.ProjectName == projeadi).Select(x => x.ProjectId).First();
            var FonksiyonID = dbContext.functions.Where(x => x.FunctionName == fonksiyonAdi && projeID == x.ProjectId)
                .Select(x => x.FunctionId).First();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkbox = (DataGridViewCheckBoxCell)row.Cells["CheckBoxColumn"];
                if (Convert.ToBoolean(checkbox.Value))
                {
                    // Her satırdaki combobox'ı al
                    DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)row.Cells["CategoryColumn"];
                    if (comboBoxCell.Value != null)
                    {
                        string selectedCategory = comboBoxCell.Value.ToString();
                        var KategoriTime = dbContext.categories.Where(x => x.CategoryName == selectedCategory).Select(x => x.CategoryTime).FirstOrDefault();
                        var KategoriID = dbContext.categories.Where(x => x.CategoryName == selectedCategory).Select(x => x.CategoryID).FirstOrDefault();

                        // Her satırdaki diğer hücrelerin değerlerini al
                        string moduleName = row.Cells["ModuleName"].Value.ToString();
                        string moduleDescription = row.Cells["ModuleDescription"].Value.ToString();
                        string moduleTip = row.Cells["ModuleTip"].Value.ToString();
                        string description = row.Cells["Description"].Value.ToString();

                        bool isModuleExists = dbContext.modules
                            .Any(m =>
                                m.ProjectId == projeID &&
                                m.FunctionId == FonksiyonID &&
                                m.ModuleName == moduleName &&
                                (m.ModuleTip == "3D" || m.ModuleTip == "2D"));

                        bool isRecordExists = dbContext.records
                            .Any(r =>
                                r.ProjectName == projeadi &&
                                r.FunctionName == fonksiyonAdi &&
                                r.ModuleName == moduleName &&
                                r.SystemName== SistemAdi &&
                                (r.ModuleTip == "3D" || r.ModuleTip == "2D"));

                        // Alınan değerleri kullanmak için burada işlem yapabilirsiniz

                        if (!isModuleExists && !isRecordExists)
                        {
                            var yeniModul = new Modules
                            {
                                FunctionId = FonksiyonID,
                                ProjectId = projeID,
                                ModuleName = moduleName,
                                ModuleDescription = moduleDescription,
                                CategoryId =KategoriID,
                                ModuleTip = moduleTip,
                                Description = description
                            };
                            var RecordEt = new Records
                            {
                                SystemName = SistemAdi,
                                ProjectName = projeadi,
                                FunctionName = fonksiyonAdi,
                                ModuleName = moduleName,
                                ModuleDescription = moduleDescription,
                                CategoryName = selectedCategory,
                                CategoryTime = KategoriTime,
                                ModuleTip = moduleTip
                            };

                            dbContext.modules.Add(yeniModul); // Yeni modulu Moduls tablosuna ekleyin

                            dbContext.records.Add(RecordEt);
                            dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                        }
                        else
                        {
                            MessageBox.Show("Bu modül zaten mevcut.");
                        }
                    }
                }
            }
        }
    }
}
