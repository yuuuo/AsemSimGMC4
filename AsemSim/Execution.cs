using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsemSim
{
    public partial class Form1 : Form
    {
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
            if (address > 97)
            {
                timer1.Stop();
                return;
            }
            CodeExecution();
        }

        /// <summary>
        /// 命令実行部
        /// </summary>
        private void CodeExecution()
        {
            int tmp;
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
                case 'E':
                    // 2バイト命令
                    address++;
                    ECodeExecution();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// サブルーチン2バイト命令を実行する関数
        /// </summary>
        private void ECodeExecution()
        {
            switch (mem[address])
            {
                case '0':
                    SetSevenLED(-1);
                    break;
                case '1':
                    SetBinaryLED(true, yr);
                    break;
                case '2':
                    SetBinaryLED(false, yr);
                    break;
                case '3':
                    ar ^= 0xF;
                    break;
                case '4':
                    break;
                case '5':
                    break;
                case '6':
                    break;
                case '7':
                    break;
                case '8':
                    break;
                case '9':
                    break;
                case 'A':
                    break;
                case 'B':
                    break;
                case 'C':
                    break;
                case 'D':
                    break;
                case 'E':
                    break;
                case 'F':
                    break;
                default:
                    break;
            }
            address++;
        }
    }
}
