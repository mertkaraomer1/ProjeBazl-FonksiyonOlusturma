namespace FonksiyonOlusturma
{
    public partial class GirisSayfası : Form
    {
        public GirisSayfası()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            systems sys = new systems();
            sys.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            KullaniciEkranı KE = new KullaniciEkranı();
            KE.Show();

        }


        private void GirisSayfası_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapatılmak istendiğinde
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Programı kapat
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rapor rp = new Rapor();
            rp.Show();
        }


    }
}
