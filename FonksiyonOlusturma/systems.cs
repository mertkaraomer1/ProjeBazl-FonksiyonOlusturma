using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using Microsoft.Office.Interop.Excel;
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
    public partial class systems : Form
    {
        private MyDbContext dbContext;
        public systems()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sistemKodu = textBox1.Text;
            try
            {
                if (!string.IsNullOrEmpty(sistemKodu)) // TextBox boş değilse
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
                    {
                        // Aynı SystemName'e sahip bir kayıt zaten var mı kontrol edin
                        var existingSystem = dbContext.systems.FirstOrDefault(s => s.SystemName == sistemKodu);

                        if (existingSystem == null)
                        {
                            // SystemName değeri özgünse yeni bir sistem ekleyin
                            var yeniSistem = new Systems
                            {
                                SystemName = sistemKodu
                                // Diğer alanlara da değer atayabilirsiniz, gerekirse.
                            };
                            dbContext.systems.Add(yeniSistem); // Yeni sistem nesnesini Systems tablosuna ekleyin
                            dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                        }
                        else
                        {
                            MessageBox.Show("Aynı SystemName ile kayıtlı sistem zaten mevcut.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sistem kodu girmelisiniz."); // TextBox boşsa kullanıcıya uyarı verin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            textBox1.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            SistemAtama();
        }

        private void systems_Load(object sender, EventArgs e)
        {
            SistemAtama();
        }
        public void SistemAtama()
        {
            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "SİSTEM KODU");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                // Projects tablosundaki verileri sorgulayın
                var sistem = dbContext.systems.ToList();

                int satirNo = 1; // Başlangıç satır numarası 1 olarak ayarlanır

                // DataGridView'e satır ekleyin
                foreach (var item in sistem)
                {
                    // DataGridView'e yeni bir satır ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // İlk sütunu (SATIR NO) satır numarası olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // İkinci sütunu (PROJE KODU) proje kodu olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = item.SystemName });

                    // DataGridView'e satırı ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her satır ekledikten sonra satır numarasını arttırın
                }
                // DataGridView kontrolünüze bir buton sütunu ekleyin.
                DataGridViewImageColumn buttonColumn = new DataGridViewImageColumn();
                buttonColumn.HeaderText = "SİL"; // Sütun başlığı
                buttonColumn.Image = Image.FromFile("delete.png"); // Silme resmini belirtin
                buttonColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Resmi düzgün görüntülemek için ayar
                dataGridView1.Columns.Add(buttonColumn);
                dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            }

        }
        public Form1 F1;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                if (F1 == null || F1.IsDisposed)
                {
                    F1 = new Form1();
                    DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string cellValue = clickedCell.Value.ToString();

                    F1.TextBoxValue = cellValue; // Form2'deki TextBox'a değeri aktar
                    F1.Show();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (columnIndex == dataGridView1.Columns[2].Index) // "Sistem Kodu" sütununda tıklama işlemi
            {
                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                {
                    string systemName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();

                    try
                    {
                        using (var dbContext = new MyDbContext())
                        {
                            var systemToDelete = dbContext.systems.FirstOrDefault(m => m.SystemName == systemName);

                            if (systemToDelete != null)
                            {
                                dbContext.systems.Remove(systemToDelete);
                                dbContext.SaveChanges();

                                // DataGridView'den seçili satırı kaldırın
                                dataGridView1.Rows.Remove(dataGridView1.Rows[rowIndex]);

                                MessageBox.Show("Sistem başarıyla silindi.");
                            }
                            else
                            {
                                MessageBox.Show("Silinecek bir şey bulunamadı.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                }
            }


        }
    }
}
