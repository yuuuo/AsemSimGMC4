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

namespace AsemSimNSS
{
    public partial class Form1 : Form
    {
        int address = 0;
        char[] mem = new char[97];

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < mem.Length - 1; i++)
            {
                mem[i] = 'F';
            }
            mem[mem.Length - 1] = '\0';
            setMemText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            address++;
            SetBinaryLED(address);
            SetSevenLED(address);
        }

        private void setMemText()
        {
            memTextBox.Text = new string(mem);
        }

        private void SetBinaryLED(int a)
        {
            PictureBox[] binaryLED = { binaryLED0, binaryLED1, binaryLED2, binaryLED3, binaryLED4, binaryLED5, binaryLED6 };
            for (int i = 0; i < binaryLED.Length; i++)
            {
                binaryLED[i].BackColor = ((a >> i) & 1) == 1 ? Color.Red : Color.Black;
            }
        }

        private void SetSevenLED(int a)
        {
            List<List<String>> match = new List<List<String>>();
            match.Add(new List<String> { "0", "2", "3", "5", "6", "7", "8", "9", "A", "E", "F" });
            match.Add(new List<String> { "0", "1", "2", "3", "4", "7", "8", "9", "A", "D" });
            match.Add(new List<String> { "0", "1", "3", "4", "5", "6", "7", "8", "9", "A", "B", "D" });
            match.Add(new List<String> { "0", "2", "3", "5", "6", "8", "9", "B", "C", "D", "E" });
            match.Add(new List<String> { "0", "2", "6", "8", "A", "B", "C", "D", "E", "F" });
            match.Add(new List<String> { "0", "4", "5", "6", "8", "9", "A", "B", "E", "F" });
            match.Add(new List<String> { "2", "3", "4", "5", "6", "8", "9", "A", "B", "C", "D", "E", "F" });
            PictureBox[] sevenLED = { sevenLED0, sevenLED1, sevenLED2, sevenLED3, sevenLED4, sevenLED5, sevenLED6 };
            for (int i = 0; i < sevenLED.Length; i++)
            {
                sevenLED[i].BackColor = match[i].FindAll(s => s == a.ToString("X")).Count != 0 ? Color.Red : Color.Black;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            address = 0;
            SetBinaryLED(address);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            StreamReader sr = new StreamReader(openFileDialog.FileName, System.Text.Encoding.GetEncoding("shift-jis"));
            sourceTextBox.Text = sr.ReadToEnd();
            sr.Close();
        }

        private void saveFileMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
            sw.Write(sourceTextBox.Text);
            sw.Close();
        }

        private void buttonStartAsm_Click(object sender, EventArgs e)
        {
            string[] line = sourceTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            char[] del = { ' ', '\t' };
            int startLine;
            for (int i = 0; i < line.Length; i++)
            {
                string[] term = line[i].Split(del, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
