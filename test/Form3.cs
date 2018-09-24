using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtoRestart;

namespace OtoRestart
{
    public partial class Form3 : Form
    {
        
        public static bool turn = false;
        public static bool turn2 = false;
        public static string passAR;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            logText("Acil Restart Tıklandı.");
            try
            {

                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://berkpekatik.com/password.txt");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                passAR = content;
            }
            catch (Exception)
            {
                logText("Acil Restart'a Erişim Rededildi.");
                MessageBox.Show("Erişim Rededildi.", "");
                MessageBox.Show("© Copyright Berk Pekatik ");
                Application.Exit();
            }

        }
        public void logText(string log)
        {
            string dosya_yolu = Application.StartupPath + @"\Log.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(dosya_yolu, true);
            sw.WriteLine("# " + log + " " + DateTime.Now.ToString());
            sw.Flush();
            sw.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (passAR == textBox1.Text)
            {
                if (Application.OpenForms.OfType<Form>().Any(f => f is Form5))
                {

                }
                else
                {
                    Form5 f5 = new Form5();
                    f5.Show();
                }
                if (Application.OpenForms.OfType<Form>().Any(f => f is Form4))
                {

                }
                else
                {
                    Form4 f4 = new Form4();
                    f4.Show();
                }
                turn = true;
                turn2 = true;
                logText("Sunucuya Acil Restart Atıldı.");
                //Form2.backupData();
                this.Hide();
            }
            else
            {
                logText("Acil Restart Şifresi Yanlış Girildi.");
                MessageBox.Show("Şifre Yanlış.");
                this.Hide();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

    }
}
