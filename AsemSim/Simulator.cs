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
        /// レジスタ，データメモリ，キー入力，実行フラグ変数宣言，補助レジスタ(_)
        /// </summary>
        int ar = 0xF;
        int br = 0xF;
        int yr = 0xF;
        int zr = 0xF;
        int ar_ = 0xF;
        int br_ = 0xF;
        int yr_ = 0xF;
        int zr_ = 0xF;
        // 0xFで初期化
        int[] dm = (new int[16]).Select(v => 15).ToArray();
        int key = -1;
        bool exFlag = true;
        // TIMRで待つ回数 -1 : 未使用
        int waitTimer = -1;

        /// <summary>
        /// レジスタの値をクリア
        /// </summary>
		private void resetRegister()
		{
			ar = 0xF;
			br = 0xF;
			yr = 0xF;
			zr = 0xF;
			ar_ = 0xF;
			br_ = 0xF;
			yr_ = 0xF;
			zr_ = 0xF;
			for(int i = 0; i < dm.Length; i++)
			{
				dm[i] = 0xf;
			}
		}


        /// <summary>
        /// ボタンが押されたときのイベント
        /// </summary>
        /// <param name="sender">押されたボタン</param>
        /// <param name="e"></param>
        private void numKey_mouseDown(object sender, EventArgs e)
        {
            //押されたボタンのテキストを代入
            key = ((Button)sender).Text.ToCharArray()[0].ToInt();
        }

        private void numKey_mouseUp(object sender, MouseEventArgs e)
        {
            key = -1;
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
        /// 2進LEDの一箇所を表示
        /// </summary>
        /// <param name="o">出力</param>
        /// <param name="bit">2進LEDのビット番号</param>
        private void SetBinaryLED(bool o, int bit)
        {
            PictureBox[] binaryLED = { binaryLED0, binaryLED1, binaryLED2, binaryLED3, binaryLED4, binaryLED5, binaryLED6 };
            binaryLED[bit].BackColor = o ? Color.Red : Color.Black;
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
