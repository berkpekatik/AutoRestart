using OtoRestart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {


        public static string pass;
        public static string copy = "Berk Pekatik (berkpekatik.com)";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            logText("Program Açıldı.");
            try
            {
                
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://berkpekatik.com/password.txt");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                pass = content;
            }
            catch (Exception)
            {
                logText("Erişim Rededildi.");
                MessageBox.Show("Erişim Rededildi.", "");
                MessageBox.Show(copy, "© Copyright ");
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == pass)
            {
                this.Hide();
                logText("Başarıyla Giriş Yapıldı.");
                Form2 frm = new Form2();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Şifre Hatalı.", "");
                MessageBox.Show(copy, "© Copyright ");
                Application.Exit();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
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
    }
}
