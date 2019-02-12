using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsemSim
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// アセンブリスタートボタンイベント
        /// </summary>
        private void buttonStartAsm_Click(object sender, EventArgs e)
        {
            //メモリを初期化
            for (int i = 0; i < mem.Length - 1; i++)
            {
                mem[i] = 'F';
            }

            //1行分のデータに分割
            string[] line = sourceTextBox.Text.ToUpper().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            //区切り文字
            char[] del = { ' ', '\t' };

            //スタート行探索
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
            //ラベル保存用辞書
            Dictionary<string, string> asmLabelDic = new Dictionary<string, string>();

            //Pass 1
            for (int i = startLine + 1; i < line.Length; i++)
            {
                //1ワードごとにキューに入れる
                Queue<string> term = new Queue<string>(line[i].ToUpper().Split(del, StringSplitOptions.RemoveEmptyEntries));
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
                else if (opc == "DC")
                {

                }
                else if (opc[0] == ';')
                {

                }
                else
                {
                    adr += OperationArray.op[opc].length;
                }
            }

            //++adrしてから読むため-1
            adr = -1;

            // DC用カウンタ
            int DCCounter = 0;
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
                else if (opc == "DC")
                {
                    opr = term.Dequeue();
                    mem[80 + DCCounter++] = opr[0];
                    continue;
                }
                else if (opc[0] == ';')
                {
                    continue;
                }

                //命令を命令コードに変換
                mem[++adr] = OperationArray.op[opc].code;

                //更に引数がある場合
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

            //結果を出力
            setMemText();
        }
    }
}
