using FonksiyonOlusturma.MyDb;
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
    public partial class Admin : Form
    {
        private MyDbContext dbContext;
        public Admin()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Kullanıcı adı ve şifre kontrolü için LINQ sorgusu
            var user = dbContext.accounts.FirstOrDefault(a => a.KullaniciAdi == username && a.Sifre == password);

            if (user != null)
            {
                // Kullanıcı doğrulandı, istediğiniz formu açabilirsiniz
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış.");
            }


        }
    }
}
