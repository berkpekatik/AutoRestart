using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtoRestart;

namespace OtoRestart
{
    public partial class Form5 : Form
    {

        public static Process p = new Process();

        public Form5()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        private const int WM_SYSCOMMAND = 274;
        private const int SC_MAXIMIZE = 61488;

        

        private void Form5_Load(object sender, EventArgs e)
        {
            
            serverOn1();
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }

            if (Form3.turn == true)
            {
                Show();
                Form3.turn = false;
                p.CloseMainWindow();
                serverOn1();
            }
            if (Form2.turn == true)
            {
                Show();
                Form2.turn = false;
                p.CloseMainWindow();
                serverOn1();
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("2.Sunucu Ekranı Kapanacak Emin misin ?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                
                DialogResult dialogResult2 = MessageBox.Show("2.Sunucu ayrı bir pencerede çalıştırılsın mı ?", "Sunucu", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    SetParent(p.MainWindowHandle, IntPtr.Zero);
                }
                else if (dialogResult2 == DialogResult.No)
                {
                    timer1.Stop();
                    p.CloseMainWindow();
                }
                    
            }
            else if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = true;
        }

        private void açToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            serverOn1();
            timer1.Start();
        }

        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            DialogResult dialogResult = MessageBox.Show("2.Sunucu Kapatılıyor, Emin misin ?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                timer1.Stop();
                p.CloseMainWindow();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
            
        }
        public void serverOn1()
        {
            Show();
            p.StartInfo.FileName = "C:\\Sunucu\\run.cmd";
            p.StartInfo.Arguments = "+exec server2.cfg";
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p.Start();
            Thread.Sleep(500); // Allow the process to open it's window
            SetParent(p.MainWindowHandle, panel1.Handle);
            timer1.Start();
        }

       
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Show();
            p.CloseMainWindow();
            serverOn1();
        }

        private void kapatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void degisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
