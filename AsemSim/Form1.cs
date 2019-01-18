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

namespace AsemSim
{
    public partial class Form1 : Form
    {
        int address = 0;
        char[] mem = new char[97];

        Dictionary<string, string> asmLabelDic = new Dictionary<string, string>();

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
            int startLine = 0;
            for (int i = 0; i < line.Length; i++)
            {
                string[] term = line[i].Split(del, StringSplitOptions.RemoveEmptyEntries);
                if (term[0] == "START")
                {
                    startLine = i;
                    break;
                }
            }

            int adr = 0;
            int endLine = 0;
            string opc = "";
            string opr = "";
            //Pass 1
            for (int i = startLine + 1; i < line.Length; i++)
            {
                Queue<string> term = new Queue<string>(line[i].Split(del, StringSplitOptions.RemoveEmptyEntries));
                string label = "";
                //Check Label
                if (line[i].IndexOf(opc = term.Dequeue()) == 0)
                {
                    asmLabelDic.Add(opc, adr.ToString("X2"));
                    label = opc + " ";
                    opc = term.Dequeue();
                }

                if (opc == "END")
                {
                    endLine = i;
                    break;
                }
                else if (opc == "RET")
                {

                }
                else
                {
                    adr += OperationArray.op[opc].length;
                }
            }
            adr = -1;
            //Pass 2
            for (int i = startLine + 1; i < line.Length; i++)
            {
                Queue<string> term = new Queue<string>(line[i].Split(del, StringSplitOptions.RemoveEmptyEntries));
                //Check Label
                if (line[i].IndexOf(opc = term.Dequeue()) == 0)
                {
                    opc = term.Dequeue();
                }

                if (opc == "END")
                {
                    break;
                }

                mem[++adr] = OperationArray.op[opc].code;
                if (opc == "JUMP")
                {
                    opr = term.Dequeue();
                    mem[++adr] = asmLabelDic[opr][0];
                    mem[++adr] = asmLabelDic[opr][1];
                }
                else if (opc == "RET")
                {
                    mem[++adr] = 'F';
                    mem[++adr] = 'F';
                }
                else if (opc == "CAL")
                {
                    opr = term.Dequeue();
                    mem[++adr] = OperationArray.op[opr].code;
                }
                else if (OperationArray.op[opc].length >= 2)
                {
                    opr = term.Dequeue();
                    mem[++adr] = opr[0];
                }
            }
            memTextBox.Text = new string(mem);
        }
    }
}
