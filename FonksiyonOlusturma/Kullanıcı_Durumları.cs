using FonksiyonOlusturma.MyDb;
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
    public partial class Kullanıcı_Durumları : Form
    {
        MyDbContext dbContext;
        public Kullanıcı_Durumları()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void Kullanıcı_Durumları_Load(object sender, EventArgs e)
        {
            Grafik();
        }
        public void Grafik()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // Sütun başlıkları ve kalın fontu için DataGridViewCellStyle oluştur
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView1.Columns.Add("Column0", "Staff Name");
            dataGridView1.Columns.Add("Column1", "Total time");
            dataGridView1.Columns.Add("Column2", "Days to Complete");
            dataGridView1.Columns.Add("Column3", "Finish Date");


            // Sütun başlıklarına kalın fontu uygula
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style = columnHeaderStyle;
            }

            var staffNames = dbContext.staffs.Select(s => s.StaffName).ToList();

            dataGridView1.Rows.Clear();

            foreach (var staffName in staffNames)
            {
                var assignmentsForStaff = dbContext.assignments
                    .Where(a => a.StaffName == staffName && a.Status == "True") // FinishTime'ı null olanları filtrele
                    .ToList()
                    .Select(a => new
                    {
                        StaffName = a.StaffName,
                        CategoryTime = a.CategoryTime,
                    })
                    .ToList();

                int totalCategoryTime = 0;

                foreach (var assignment in assignmentsForStaff)
                {
                    totalCategoryTime += assignment.CategoryTime;
                }

                int daysToComplete = (int)Math.Ceiling((double)totalCategoryTime / 8); // Toplam iş süresini günlük işe çevirme

                // Başlangıç tarihine bugünden itibaren kaç gün varsa ekle
                DateTime finishDateEstimated = DateTime.Now.AddDays(daysToComplete);

                // DataGridView'e ekle
                dataGridView1.Rows.Add(staffName, totalCategoryTime, daysToComplete, finishDateEstimated.ToShortDateString());
            }
        }


    }
}
