using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test;

namespace OtoRestart
{
    public partial class Form2 : Form
    {
        public static string[] autoRestart = new string[10];
        public static StreamReader config = new StreamReader(Application.StartupPath + "\\resConfig.cfg");
        public static string passF3;
        public Form4 f4 = new Form4();
        public Form5 f5 = new Form5();
        public static bool turn = false;
        public static bool turn2 = false;


        public Form2()
        {
            InitializeComponent();

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Application.OpenForms.OfType<Form>().Any(f => f is Form4) || Application.OpenForms.OfType<Form>().Any(f => f is Form5))
            {
                MessageBox.Show("Çalışan sunucular var, lütfen kapatınız.");
                e.Cancel = true;
                return;
            }
            else
            {
                Application.Exit();
                logText("Program'dan Çıkış Yapıldı.");
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            string copy = "Berk Pekatik (berkpekatik.com)";
            AutoClosingMessageBox.Show(copy, "© Copyright ", 2000);
            resConf();
            timer1.Start();
            timer2.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {



            label2.Text = DateTime.Now.ToLongTimeString();
            if (DateTime.Now.ToLongTimeString() == autoRestart[0] || DateTime.Now.ToLongTimeString() == autoRestart[1] || DateTime.Now.ToLongTimeString() == autoRestart[2] || DateTime.Now.ToLongTimeString() == autoRestart[3] || DateTime.Now.ToLongTimeString() == autoRestart[4] || DateTime.Now.ToLongTimeString() == autoRestart[5] || DateTime.Now.ToLongTimeString() == autoRestart[6] || DateTime.Now.ToLongTimeString() == autoRestart[7] || DateTime.Now.ToLongTimeString() == autoRestart[8] || DateTime.Now.ToLongTimeString() == autoRestart[9])
            {


                if (checkBox1.Checked && checkBox2.Checked)
                {
                    Restart();
                    turnoffControl();
                    turn = true;
                    turn2 = true;
                    logText("1 ve 2.Sunucuya Restart Atıldı.");
                }
                else if (checkBox1.Checked)
                {
                    Restart();
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
                    logText("1.Sunucuya Restart Atıldı.");
                }
                else if (checkBox2.Checked)
                {
                    Restart();
                    if (Application.OpenForms.OfType<Form>().Any(f => f is Form5))
                    {

                    }
                    else
                    {
                        Form5 f5 = new Form5();
                        f5.Show();
                    }
                    turn = true;
                    turn2 = true;
                    logText("2.Sunucuya Restart Atıldı.");
                }
                else
                {
                    Restart();
                    turnoffControl();
                    turn = true;
                    turn2 = true;
                    //backupData();
                    AutoClosingMessageBox.Show("2 Sunucuya restart atıldı. Lütfen işaretlemeyi unutmayın.", "", 1000);
                    logText("Sorumsuzluk bu işaretlemeyi unutmuşlar :)");
                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }
        public static void backupData()
        {
            try
            {
                string constring = "server=localhost;user=root;pwd='';database=essentialmode;SslMode=none;charset=latin5;";
                string fileName = String.Format("{0:d/M/yyyy HH/mm/ss}", DateTime.Now);
                string file = Application.StartupPath + "(" + fileName + ").sql";
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(file);
                            conn.Close();
                        }
                    }
                }
                logText("Mysql Yedeklendi.");
                AutoClosingMessageBox.Show("Yedek Alındı.", "Mysql", 500);
            }
            catch (Exception)
            {
                logText("Mysql HATA.");
                AutoClosingMessageBox.Show("Mysql Yedeği Alınamadı. Lütfen Yedek Klasörünün Olduğundan Emin Olunuz.", "", 500);
            }

        }
        public static void Restart()
        {
            try
            {
                System.Diagnostics.Process.Start("CMD.exe", "/c taskkill /f /im cmd.exe");
                System.Diagnostics.Process.Start("CMD.exe", "/c taskkill /f /im conhost.exe");
                System.Diagnostics.Process.Start("CMD.exe", "/c taskkill /f /im FXServer.exe");
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception)
            {
                logText("Manuel Restart Hata:");
                AutoClosingMessageBox.Show("Restart Hatası Lütfen Yapımcıyla İrtibat'a geçiniz Hata Kodu:#111RE.", "", 10000);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //backupData();                       
        }


        private void timer2_Tick(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = true;
        }
        public void refreshListBox()
        {
            for (int i = 0; i < 9; i++)
            {
                if (autoRestart[i] == null || autoRestart[i] == "")
                {

                }
                else
                {
                    listBox1.Items.Add(autoRestart[i]);
                }
            }
        }
        public void resConf()
        {
            
            string metin = config.ReadLine();
            int sayac = 0;
            while (metin != null)
            {
                listBox1.Items.Add(metin);
                autoRestart[sayac++] = metin;
                metin = config.ReadLine();
            }
            
        }
        public static void logText(string log)
        {
            string dosya_yolu = Application.StartupPath + @"\Log.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(dosya_yolu, true);
            sw.WriteLine("# " + log + " " + DateTime.Now.ToString());
            sw.Flush();
            sw.Close();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        public void turnoffControl()
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

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void sunucuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form>().Any(f => f is Form4))
            {
                MessageBox.Show("1.Sunucu Açık Kontrol Edin.");
            }
            else
            {
                Form4 f4 = new Form4();
                f4.Show();
            }
        }

        private void sunucu2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form>().Any(f => f is Form5))
            {
                MessageBox.Show("2.Sunucu Açık Kontrol Edin.");
            }
            else
            {
                Form5 f5 = new Form5();
                f5.Show();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int deger1 = listBox1.SelectedIndex;
            if (deger1 != -1)
            {
                if (autoRestart[deger1].First().ToString() == "#")
                {
                    MessageBox.Show("Zaten İptal Edilmiş.");
                }
                else
                {
                    autoRestart[deger1] = "#" + autoRestart[deger1];
                }

            }
            listBox1.Items.Remove(listBox1.SelectedItem);
            listBox1.Items.Clear();
            refreshListBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int deger1 = listBox1.SelectedIndex;
            if (deger1 != -1)
            {
                if (autoRestart[deger1].First().ToString() == "#")
                {
                    string word = autoRestart[deger1].ToString();
                    string wordparse = word.Substring(1, word.Length - 1);
                    autoRestart[deger1] = wordparse;
                }
                else
                {
                    MessageBox.Show("Zaten Aktif Edilmiş.");
                }

            }
            listBox1.Items.Remove(listBox1.SelectedItem);
            listBox1.Items.Clear();
            refreshListBox();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}