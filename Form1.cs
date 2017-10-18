using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATCrackGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) // Proxy List
        {
            if (openProxy.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = System.IO.Path.GetFileName(openProxy.FileName);
            }
            else
            {
                MessageBox.Show("File not found!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Account List
        {
            if (openAccount.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = System.IO.Path.GetFileName(openAccount.FileName);
            }
            else
            {
                MessageBox.Show("File not found!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e) // Save to - Finished
        {
            if (saveFinished.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = System.IO.Path.GetFileName(saveFinished.FileName);
            }
            else
            {
                MessageBox.Show("An error occured during file selection!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadLicense();
            checkDeps();
        }

        private void ReadLicense()
        {
            if (File.Exists(Environment.CurrentDirectory + "/license.txt") && File.ReadAllText(Environment.CurrentDirectory + "/license.txt").Length == 24)
            {
                licenseBox.Text = File.ReadAllText(Environment.CurrentDirectory + "/license.txt");
            }else if (!File.Exists(Environment.CurrentDirectory + "/license.txt"))
            {
                File.Create(Environment.CurrentDirectory + "/license.txt");
            }
        }

        private void checkDeps()
        {
            if (!File.Exists(Environment.CurrentDirectory + "/Cracker.jar"))
            {
                MessageBox.Show("Cracker.jar not found!\nMake sure they are in the same folder!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (licenseBox.Text.Length == 24 && File.Exists(openAccount.FileName) && File.Exists(openProxy.FileName) && saveFinished.CheckPathExists)
            {
                File.WriteAllText(Environment.CurrentDirectory + "/license.txt", licenseBox.Text);
                string args = " -jar Cracker.jar -l " + licenseBox.Text + " -c " + openAccount.FileName + " -p " +
                              openProxy.FileName + " -f " + saveFinished.FileName;
                if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[0]) && saveUsernames.FileName != "saveUsernames")
                {
                    args = args + " -u " + saveUsernames.FileName;
                }
                if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[1]))
                {
                    args = args + " -bc";
                }
                if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[2]))
                {
                    args = args + " -n";
                }
                else if(checkedListBox1.GetItemCheckState(0) == CheckState.Checked && saveUsernames.FileName != "saveUsernames")
                {
                    MessageBox.Show("Username save path not found!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process.Start("java", args);
                Environment.Exit(0);

            }
            else if (licenseBox.Text.Length != 24)
            {
                MessageBox.Show("Invalid License!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Invalid File Path!", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsRunning()
        {
            Process[] plist = Process.GetProcesses();
            foreach (Process p in plist)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains("Accounts Today Cracker"))
                {
                    DialogResult dr = MessageBox.Show("ATCrack is already running!\nWould you like me to close it?", "Oops!", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Yes)
                    {
                        p.Kill();
                        return true;
                    }
                    else if (dr == DialogResult.No)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private void button5_Click(object sender, EventArgs e) // Save to - Usernames
        {
            if (saveUsernames.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox4.Text = System.IO.Path.GetFileName(saveUsernames.FileName);
            }
            else
            {
                MessageBox.Show("An error occured during file selection!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void checkedListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[0]))
            {
                textBox4.Enabled = true;
                button5.Enabled = true;
            }
            else if (!checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[0]))
            {
                textBox4.Enabled = false;
                button5.Enabled = false;
            }
        }
    }
}
