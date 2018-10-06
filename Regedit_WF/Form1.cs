using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Regedit_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey key = currentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            string info = key.GetValue(listBox1.SelectedItem.ToString()).ToString();

            MessageBox.Show("Значение ключа реестра\n"+info);

            key.Close();
        }

        private void BtnGetList_Click(object sender, EventArgs e)
        {
            AllRun();      
        }

        private void AllRun()
        {
            listBox1.Items.Clear();
            try
            {
                RegistryKey currentUser = Registry.CurrentUser;
                RegistryKey key = currentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                foreach (var item in key.GetValueNames())
                {
                    listBox1.Items.Add(item);
                }
                key.Close();
                currentUser.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {
                RegistryKey currentUser = Registry.CurrentUser;
                RegistryKey key = currentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                RegistryKey newkey = key.CreateSubKey(txtBoxAddName.Text);
                string str = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);

                key.SetValue(str, openFileDialog1.FileName);
                key.Close();
                currentUser.Close();
                AllRun();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey key = currentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key.DeleteValue(listBox1.SelectedItem.ToString());
            MessageBox.Show("Запись "+ listBox1.SelectedItem.ToString()+" удалена из реестра");
            key.Close();
            currentUser.Close();
            AllRun();
        }
    }
}
