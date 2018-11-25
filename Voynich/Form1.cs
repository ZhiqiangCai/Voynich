using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Voynich
{
    public partial class Form1 : Form
    {
        string corpusPath = "..\\data\\voynich_original";
        FileInfo[] corpusFiles;
        public Form1()
        {
            InitializeComponent();
        }

        private void openCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openCorups();
        }
        private void openCorups()
        {
            if (corpusFiles != null) return;
            DirectoryInfo dir = new DirectoryInfo(corpusPath);
            corpusFiles = dir.GetFiles("*.txt");
            foreach (FileInfo f in corpusFiles)
            {
                this.listBox1.Items.Add(f.Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.Items.Count == 0) return;
            if (this.listBox1.SelectedIndex > -1)
            {
                this.richTextBox1.Clear();
                this.richTextBox1.LoadFile(corpusFiles[this.listBox1.SelectedIndex].FullName,RichTextBoxStreamType.PlainText);
            }
        }

        private void wordCountToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Dictionary<string, int> dict = countWords();
            this.richTextBox1.Clear();
            foreach (string t in dict.Keys)
            {
                this.richTextBox1.AppendText(t + "\t" + dict[t] + "\r\n");
            }
        }
        Dictionary<string, int> countWords()
        {
            if (corpusFiles == null) openCorups();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (FileInfo f in corpusFiles)
            {
                string text = "";
                using (StreamReader sr = new StreamReader(f.FullName))
                {
                    text = sr.ReadToEnd();
                }
                string[] tokens = text.Trim().Split(" \t\r\n,-_=".ToArray());
                foreach (string t in tokens)
                {
                    if (!dict.ContainsKey(t)) dict.Add(t, 1);
                    else dict[t]++;
                }
            }
             
            return dict;
        }
        private void randCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> dict = countWords();
            HashSet<int> hash = new HashSet<int>();
            foreach(string s in dict.Keys)
            {
                hash.Add(dict[s]);
            }
            this.richTextBox1.Clear();
            foreach (int n in hash)
            {
                this.richTextBox1.AppendText(n + "\r\n");
            }
        }
    }
}
