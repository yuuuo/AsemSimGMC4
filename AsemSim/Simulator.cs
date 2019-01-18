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
using System.Globalization;

namespace AsemSim
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// レジスタ，データメモリ，キー入力，実行フラグ変数宣言
        /// </summary>
        int ar = 0xF;
        int br = 0xF;
        int yr = 0xF;
        int zr = 0xF;
        int[] dm = (new int[16]).Select(v => 15).ToArray();
        int key = -1;
        bool exFlag = true;

        /// <summary>
        /// 実行ボタンイベント
        /// </summary>
        private void buttonRun_Click(object sender, EventArgs e)
        {
            timer1.Start();
            address = 0;
            exFlag = true;
        }

        /// <summary>
        /// リセットボタンイベント
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        /// <summary>
        /// タイマー割り込み
        /// シミュレーター実行部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int tmp;
            if(address > 97)
            {
                timer1.Stop();
                return;
            }
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
                    address++;
                    break;
                case '1':
                    SetSevenLED(ar);
                    address++;
                    break;
                case '2':
                    //tmp : swap
                    tmp = ar;
                    ar = br;
                    br = tmp;
                    tmp = yr;
                    zr = yr;
                    zr = tmp;
                    address++;
                    break;
                case '3':
                    //tmp : swap
                    tmp = ar;
                    ar = yr;
                    yr = tmp;
                    address++;
                    break;
                case '4':
                    dm[50 + yr] = ar;
                    address++;
                    break;
                case '5':
                    ar = dm[50 + yr];
                    address++;
                    break;
                case '6':
                    ar = (ar + dm[50 + yr]) % 15;
                    exFlag = (ar + dm[50 + yr]) / 15 > 0;
                    address++;
                    break;
                case '7':
                    //tmp : 引き算の結果 負なら : 実行フラグtrue & 0~15に変換
                    ar = (exFlag = (tmp = dm[50 + yr] - ar) < 0) ? 16 + tmp : tmp;
                    address++;
                    break;
                case '8':
                    ar = mem[address + 1].ToInt();
                    address += 2;
                    break;
                case '9':
                    ar = (ar + mem[address + 1].ToInt()) % 15;
                    exFlag = (ar + mem[address + 1].ToInt()) / 15 > 0;
                    address += 2;
                    break;
                case 'A':
                    yr = mem[address + 1].ToInt();
                    address += 2;
                    break;
                case 'B':
                    yr = (ar + mem[address + 1].ToInt()) % 15;
                    exFlag = (ar + mem[address + 1].ToInt()) / 15 > 0;
                    address += 2;
                    break;
                case 'C':
                    exFlag = ar != mem[address + 1].ToInt();
                    address += 2;
                    break;
                case 'D':
                    exFlag = yr != mem[address + 1].ToInt();
                    address += 2;
                    break;
                case 'F':
                    if (exFlag)
                    {
                        address = int.Parse(new string(new char[2] { mem[address + 1], mem[address + 2] }), NumberStyles.AllowHexSpecifier);
                    }
                    else
                    {
                        address += 3;
                        exFlag = true;
                    }
                    break;
                default:
                    break;
            }
            key = -1;
        }

        /// <summary>
        /// ボタンがクリック時のイベント
        /// </summary>
        /// <param name="sender">押されたボタン</param>
        /// <param name="e"></param>
        private void numKey_Click(object sender, EventArgs e)
        {
            //押されたボタンのテキストを代入
            key = ((Button)sender).Text.ToCharArray()[0].ToInt();
        }

        /// <summary>
        /// 2進LEDを表示
        /// </summary>
        /// <param name="a">表示する数</param>
        private void SetBinaryLED(int a)
        {
            PictureBox[] binaryLED = { binaryLED0, binaryLED1, binaryLED2, binaryLED3, binaryLED4, binaryLED5, binaryLED6 };
            for (int i = 0; i < binaryLED.Length; i++)
            {
                binaryLED[i].BackColor = ((a >> i) & 1) == 1 ? Color.Red : Color.Black;
            }
        }

        /// <summary>
        /// 7segLEDを表示
        /// </summary>
        /// <param name="a">表示する数 負の数で表示を消す</param>
        private void SetSevenLED(int a)
        {
            PictureBox[] sevenLED = { sevenLED0, sevenLED1, sevenLED2, sevenLED3, sevenLED4, sevenLED5, sevenLED6 };
            //負の数の場合全部消す
            if (a < 0) sevenLED.Select(i => i.BackColor = Color.Silver);

            //それぞれのLEDでマッチしたら赤にする
            List<List<String>> match = new List<List<String>>();
            match.Add(new List<String> { "0", "2", "3", "5", "6", "7", "8", "9", "A", "E", "F" });
            match.Add(new List<String> { "0", "1", "2", "3", "4", "7", "8", "9", "A", "D" });
            match.Add(new List<String> { "0", "1", "3", "4", "5", "6", "7", "8", "9", "A", "B", "D" });
            match.Add(new List<String> { "0", "2", "3", "5", "6", "8", "9", "B", "C", "D", "E" });
            match.Add(new List<String> { "0", "2", "6", "8", "A", "B", "C", "D", "E", "F" });
            match.Add(new List<String> { "0", "4", "5", "6", "8", "9", "A", "B", "E", "F" });
            match.Add(new List<String> { "2", "3", "4", "5", "6", "8", "9", "A", "B", "C", "D", "E", "F" });
            for (int i = 0; i < sevenLED.Length; i++)
            {
                sevenLED[i].BackColor = match[i].Any(s => s == a.ToString("X")) ? Color.Red : Color.Silver;
            }
        }
    }
}
