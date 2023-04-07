using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ix_uploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                uploadFile(textBox1.Text);
            } else {
                link.Text = "The specified file does not exist in the file system.";
                linkLabel1.Visible = false;
                linkLabel2.Visible = false;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
            if (checkBox1.Checked)
            {
                uploadFile(openFileDialog1.FileName);
            }
        }

        public void uploadFile(string file)
        {
            resultPanel.Visible = false;
            Process uploader = new Process();
            uploader.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\curl.exe";
            uploader.StartInfo.Arguments = "-F \"f:1=@\\\"" + file + "\\\"\" ix.io";
            uploader.StartInfo.CreateNoWindow = true;
            uploader.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            uploader.StartInfo.UseShellExecute = false;
            uploader.StartInfo.RedirectStandardOutput = true;
            uploader.Start();
            string output = uploader.StandardOutput.ReadToEnd();
            uploader.WaitForExit();
            resultPanel.Visible = true;
            if (uploader.ExitCode == 0)
            {
                link.Text = output;
                linkLabel1.Visible = true;
                linkLabel2.Visible = true;
            } else {
                link.Text = "Could not get upload link. \"curl\" error code: " + uploader.ExitCode;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(link.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataObject data = new DataObject();
            data.SetText(link.Text, TextDataFormat.Text);
            Clipboard.SetDataObject(data, true);
            NotifyIcon notify = new NotifyIcon();
            notify.Visible = true;
            notify.Icon = Icon;
            notify.BalloonTipTitle = "The link has been copied to the clipboard";
            notify.BalloonTipText = "Feel free to paste it anywhere";
            notify.ShowBalloonTip(3000);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/CodingWonders/ix-uploader");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/CodingWonders");
        }
    }
}