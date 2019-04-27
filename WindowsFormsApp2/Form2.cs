using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public int count = 0;                 
        int startIndex, lenght;
        public void CheckTemplate(string template)
        {
            bool check;
            int z;
            char symbol;
            for (int i = 0; i < template.Length; i++)
            {
                while (i < template.Length && template[i] != '}') i++;
                z = 1; check = false;
                while (i + z < template.Length && template[i + z] == template[i - 1])
                {
                    z++;
                    check = true;
                }
                if (check)
                {
                    symbol = template[i - 1];
                    template = template.Remove(i + 1, z - 1);

                    for (int k = 0; k < z - 1; k++)
                    {
                        template = template.Insert(i - 2, symbol.ToString());
                    }

                }
            }
        }
        public List<string> GetWords(string text, string template,List<string> tempWords)
        {
            int z;
            bool check;            
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n', ';' };
            string[] justWords = text.Split(delimiterChars);
            int j, l;
            if (template == "")
            {
                MessageBox.Show("Enter template", "No template");
                return tempWords = null;
            }
            else
            {
                CheckTemplate(template);
                count = 0;
                foreach (string word in justWords)
                {
                    j = 0; l = 0;
                NextChar:
                    if (j < word.Length && l < template.Length && word[j] == template[l])
                    {
                        j++; l++;
                        goto NextChar;
                    }
                    else if (j < word.Length && l < template.Length && template[l] == '{')
                    {
                        if (l + 1 < template.Length && j + 1 < word.Length && word[j] == template[l + 1] && word[j + 1] == template[l + 1])
                        {
                            while (j < word.Length && word[j] == template[l + 1])
                            {
                                j++;
                            }
                            l += 3;
                            goto NextChar;
                        }
                        else continue;
                    }

                    else if (j < word.Length && l < template.Length && template[l] == '(')
                    {
                        z = 0;
                        check = false;
                        while (template[l + z] != ')')
                        {
                            if (word[j] == template[l + z])
                            {
                                check = true;
                                j++;
                            }
                            z++;
                        }
                        if (!check) continue;
                        l += z + 1;
                        goto NextChar;
                    }
                    else if (j == word.Length && l == template.Length)
                    {
                        tempWords.Add(word);                        
                        count++;
                    }
                    else continue;
                }
                if (tempWords.Count == 0) return tempWords = null;
                return tempWords;
            }

        }
        
        public Form1 f1;        
        public Form2(Form1 frm1)
        {
            InitializeComponent();
            this.f1 = frm1;            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
      
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            changeString.Enabled = true;
            buttonNext.Enabled = true;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text, addText;
            List<string> tempWords = new List<string>(100);
            if (radioButton2.Checked)
            {
                text = f1.richTextBox1.Text.Substring(f1.richTextBox1.GetCharIndexFromPosition(f1.p));
                addText = f1.richTextBox1.Text.Substring(0, f1.richTextBox1.GetCharIndexFromPosition(f1.p));
            }
            else { text = f1.richTextBox1.Text; addText = ""; }
            string template = textBox1.Text;
            string changeTo = textBox2.Text;
            tempWords = GetWords(text, template, tempWords);
            for (int i = 0; i < count; i++)
            {
                text = text.Replace(tempWords[i], changeTo);
            }
            text = addText + text;
            f1.richTextBox1.Text = text;
            changeString.Enabled = false;
            button2.Enabled = false;
            buttonNext.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {                                              
            f1.richTextBox1.SelectedText = textBox2.Text;
            nextString = 0;
            button2.Enabled = false;            
        }

        int nextString = 0;
        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> wordsToCheck = new List<string>(100);
            string text;
            string beforeMousePosText;
            if (radioButton2.Checked)
            {
                text = f1.richTextBox1.Text.Substring(f1.richTextBox1.GetCharIndexFromPosition(f1.p));
                beforeMousePosText = f1.richTextBox1.Text.Substring(0, f1.richTextBox1.GetCharIndexFromPosition(f1.p));                
            }
            else
            {
                text = f1.richTextBox1.Text;
                beforeMousePosText = "";
            }
            string template = textBox1.Text;
            int zipZap = 1;
            wordsToCheck = GetWords(text, template, wordsToCheck);
            if (wordsToCheck != null)
            {
                if (nextString == count) nextString = 0;            
                while (nextString - zipZap > -1 && wordsToCheck[nextString - zipZap] == wordsToCheck[nextString])
                {
                    text = text.Remove(text.IndexOf(wordsToCheck[nextString]), 1);
                    text = text.Insert(text.IndexOf(wordsToCheck[nextString])-1, "?");                    
                    zipZap++;                    
                }                
                startIndex = text.IndexOf(wordsToCheck[nextString]);
                lenght = wordsToCheck[nextString].Length;
                f1.richTextBox1.SelectionStart = startIndex + beforeMousePosText.Length;
                f1.richTextBox1.SelectionLength = lenght;
                f1.richTextBox1.Focus();
                button2.Enabled = true;
                nextString++;
            }
            else
            {
                buttonNext.Enabled = false;
                changeString.Enabled = false;
            }
        }
    }
}
