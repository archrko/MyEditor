using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }

        bool saved = false;

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saved = true;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(richTextBox1.Text);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            saved = false;
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                if(MessageBox.Show("Changes were not saved. Exit?", "Warning", MessageBoxButtons.OKCancel)== DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        string CurrentFile = "";
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                if(((DialogResult.OK==MessageBox.Show("The content will be lost.", "Continue?", MessageBoxButtons.OKCancel))))
                {
                    richTextBox1.Text = "";
                    CurrentFile = "";
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(DialogResult.OK == openFileDialog.ShowDialog())
                {
                    CurrentFile = openFileDialog.FileName;
                    if (Path.GetExtension(CurrentFile) == ".txt" || Path.GetExtension(CurrentFile) == ".cs")
                        richTextBox1.LoadFile(CurrentFile, RichTextBoxStreamType.PlainText);
                    else richTextBox1.LoadFile(CurrentFile);
                    this.Text = Path.GetFileName(CurrentFile) + " - Text Editor";

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            Form2 f2 = new Form2(this);
            f2.ShowDialog();
            richTextBox1.Text = f2.f1.richTextBox1.Text;
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
         
        }
        public Point p;
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            p.X = e.X;
            p.Y = e.Y;
        }
    }
}
