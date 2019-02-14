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
        /// <summary>
        /// アドレス
        /// プログラムメモリ宣言
        /// </summary>
        int address = 0;
        char[] mem = new char[79];

        /// <summary>
        /// メモリ初期化処理
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < mem.Length - 1; i++)
            {
                mem[i] = 'F';
            }
            mem[mem.Length - 1] = '\0';
            setMemText();
            setStatus();
        }

        /// <summary>
        /// INCRボタンイベント
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            address++;
            SetBinaryLED(address);
            SetSevenLED(address);
        }

        /// <summary>
        /// プログラムメモリにセットする
        /// </summary>
        private void setMemText()
        {
            memTextBox.Text = new string(mem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// メニューバーファイルオープンの処理
        /// </summary>
        private void openFileMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            StreamReader sr = new StreamReader(openFileDialog.FileName, System.Text.Encoding.GetEncoding("shift-jis"));
            sourceTextBox.Text = sr.ReadToEnd();
            sr.Close();
        }

        /// <summary>
        /// メニューバーファイル保存の処理
        /// </summary>
        private void saveFileMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.GetEncoding("shift-jis"));
            sw.Write(sourceTextBox.Text);
            sw.Close();
        }
        /// <summary>
        /// フォーム下のレジスタ等のステータスを表示する
        /// </summary>
        private void setStatus()
        {
            statusLabel.Text = "A : " + ar.ToString("X") + " , B : " + br.ToString("X") + " , Y : " + yr.ToString("X") + " , Z : " + zr.ToString("X") +
                              " A' : " + ar_.ToString("X") + " , B' : " + br_.ToString("X") + " , Y' : " + yr_.ToString("X") + " , Z' : " + zr_.ToString("X") +
                              "  実行フラグ : " + exFlag.ToString() + " dm : ";
			foreach (var item in dm)
			{
				statusLabel.Text += item.ToChar().ToString() + ", ";
			}
        }
    }
}
