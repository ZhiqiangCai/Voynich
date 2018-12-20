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
        string corpusPath = "..\\data\\courrier";
        string fourFilesPath = "..\\data\\courrier\\fourfiles";
        string pageFilesPath = "..\\data\\courrier\\pagefiles";
        FileInfo[] corpusFiles;
        public Form1()
        {
            InitializeComponent();
          // Queue<string> queue;
          //  queue.Dequeue, queue.Enqueue, queue.Contains, queue.Clear; queue.Peek; queue.i
        }

        private void openCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openCorups();
        }
        private void openCorups()
        {
            if (corpusFiles != null) return;
            DirectoryInfo dir = new DirectoryInfo(pageFilesPath);
            corpusFiles = dir.GetFiles("*.txt");
            if (corpusFiles == null || corpusFiles.Length == 0)
            {
                ConvertCorpus();
            }
            corpusFiles = dir.GetFiles("*.txt");
            foreach (FileInfo f in corpusFiles)
            {
                this.listBox1.Items.Add(f.Name);
            }
        }
        private void ConvertCorpus()
        {
            DirectoryInfo dir = new DirectoryInfo(fourFilesPath);
            FileInfo[] fourFiles = dir.GetFiles("*.txt");
            // filter: .-=?*
            // remove undetermined tokens
            int pageCount = 0;
            int totalCount = 0;
            bool hasAddedEmptyLine = false;
            foreach(FileInfo f in fourFiles)
            {
               
                string[] nameTokens = f.Name.Split("_.".ToArray());
                string category = nameTokens[4].Trim();
                StreamWriter sw=null;
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(f.FullName))
                {
                    //int paraCount = 0;
                    int catePage = 0;
                    string label = "";
                    sb.Clear();
                    while (sr.Peek()>-1)
                    {
                        string line = sr.ReadLine().Trim();
                        if (line.StartsWith("***"))
                        {
                           if(sb.ToString().Trim()!="")
                            {
                                //paraCount++;
                                totalCount++;
                                //save(totalCount,pageCount, category, catePage, paraCount, label, sb.ToString().Trim());
                                save(totalCount, pageCount, category, catePage,label, sb.ToString().Trim());
                                sb.Clear();

                            }
                            pageCount++; catePage++;
                            //paraCount = 0;
                            label = line.Trim().Replace("***", "");
                            if (label == "") label = "noLabel";
                        }
                        else if (line == ""&&!hasAddedEmptyLine)
                        {
                            /* string para = sb.ToString().Trim();
                             sb.Clear();
                             if (para != "")
                             {
                                 paraCount++; totalCount++;
                                 save(totalCount, pageCount, category, catePage, paraCount, label, para);
                             }*/
                            sb.AppendLine();
                            sb.AppendLine();
                            hasAddedEmptyLine = true;
                        }
                        else
                        {
                            line = cleanLine(line).Trim();
                            if (line != "")
                            {
                                sb.Append(line + " ");
                                hasAddedEmptyLine = false;
                            }
                        }
                    }
                    string pEnd = sb.ToString().Trim();
                    sb.Clear();
                    if (pEnd != "")
                    {
                        //paraCount++;

                        totalCount++;
                        //save(totalCount, pageCount, category, catePage, paraCount, label, pEnd);
                        save(totalCount, pageCount, category, catePage, label, pEnd);
                    }
                }
            }
        }
        void save(int totalCount,int page,string cat, int catPage, string label, string text)
        {
            if (text.Trim() == "") return;
            string pref = "";
            if (totalCount < 10) pref = "00";
            else if (totalCount < 100) pref = "0";
            string txtFileName =pref+totalCount+"_" +page + "_" + cat + "_" + catPage + "_" + label + ".txt";
            txtFileName = txtFileName.Replace(",", "");
            using (StreamWriter sw = new StreamWriter(pageFilesPath + "\\" + txtFileName))
            {
                sw.WriteLine(text);
                sw.Close();
            }
        }
        string cleanLine(string line)
        {
            line = line.Replace("-", "");
            line = line.Replace("?", "");
            line = line.Replace("*", "");
            line = line.Replace("=", "");
            line = line.Replace("( ", "(");
            line = line.Replace(" )", ")");
            line = line.Replace(" |", "|");
            line = line.Replace("| ", "|");
            StringBuilder sb = new StringBuilder();
            string[] tokens = line.Split(' ');
            foreach(string t in tokens)
            {
                if (t.IndexOf("|") > -1) continue;
                sb.Append(t + " ");
            }
            return sb.ToString().Trim();
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
