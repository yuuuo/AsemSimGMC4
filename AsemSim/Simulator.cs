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
        int ar = 0xF;
        int key;
        bool exFlag = true;

        private void buttonRun_Click(object sender, EventArgs e)
        {
            timer1.Start();
            address = 0;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (mem[address])
            {
                case '0':
                    if (key != -1)
                    {
                        ar = key;
                        exFlag = false;
                    }
                    else
                    {
                        exFlag = true;
                    }
                    break;
                case '8':
                    ar = mem[address + 1].ToInt();
                    address += 2;
                    break;
                default:
                    break;
            }
            key = -1;
        }

        private void numKey_Click(object sender, EventArgs e)
        {
            key = ((Button)sender).Text.ToCharArray()[0].ToInt();
            SetSevenLED(key);
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
                sevenLED[i].BackColor = match[i].Any(s => s == a.ToString("X")) ? Color.Red : Color.Silver;
            }
        }
    }
}
