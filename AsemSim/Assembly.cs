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
            int startLine = -1;
            int endLine = -1;
            for (int i = 0; i < line.Length; i++)
            {
                string[] term = line[i].Split(del, StringSplitOptions.RemoveEmptyEntries);
                if (term.Count() == 0) continue;
                if (term[0] == "START")
                {
                    startLine = i;
                }
                if (term[0] == "END")
                {
                    endLine = i;
                }
            }

            if (startLine == -1) Error.Start();
            if (endLine == -1) Error.End();

            int adr = 0;
            string opc = "";
            string opr = "";
            //ラベル保存用辞書
            Dictionary<string, string> asmLabelDic = new Dictionary<string, string>();

            //Pass 1
            for (int i = startLine + 1; i < endLine; i++)
            {
                //1ワードごとにキューに入れる
                Queue<string> term = new Queue<string>(line[i].ToUpper().Split(del, StringSplitOptions.RemoveEmptyEntries));

                if (term.Count() == 0) continue;
                //Check Label
                if (line[i].IndexOf(opc = term.Dequeue()) == 0)
                {
                    try
                    {
                        asmLabelDic.Add(opc, adr.ToString("X2"));
                    }
                    catch (Exception ex)
                    {
                        Error.LabelMany(i + 1);
                    }
                    opc = term.Dequeue();
                }

                if (opc == "RET")
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
                    try
                    {
                        adr += OperationArray.op[opc].length;
                    }
                    catch (Exception ex)
                    {
                        Error.Opcode(i + 1);
                        return;
                    }
                }
            }

            //++adrしてから読むため-1
            adr = -1;

            // DC用カウンタ
            int DCCounter = 0;
            //Pass 2
            for (int i = startLine + 1; i < endLine; i++)
            {
                Queue<string> term = new Queue<string>(line[i].Split(del, StringSplitOptions.RemoveEmptyEntries));
                //Check Label
                if (line[i].IndexOf(opc = term.Dequeue()) == 0)
                {
                    opc = term.Dequeue();
                }
                if (opc == "DC")
                {
                    opr = term.Dequeue();
                    dm[DCCounter++] = opr[0].ToInt();
                    continue;
                }
                else if (opc[0] == ';')
                {
                    continue;
                }

                //命令を命令コードに変換
                try
                {
                    mem[++adr] = OperationArray.op[opc].code;
                }
                catch (Exception ex)
                {
                    Error.Opcode(i + 1);
                    return;
                }

                if (OperationArray.op[opc].length >= 2)
                {

                    //更に引数がある場合
                    if (opc == "JUMP")
                    {
                        opr = term.Dequeue();
                        try
                        {
                            mem[++adr] = asmLabelDic[opr][0];
                            mem[++adr] = asmLabelDic[opr][1];
                        }
                        catch (Exception ex)
                        {
                            Error.LabelNotFound(i + 1);
                        }

                    }
                    else if (opc == "RET")
                    {
                        mem[++adr] = 'F';
                        mem[++adr] = 'F';
                    }
                    else if (opc == "CAL")
                    {
                        opr = term.Dequeue();
                        try
                        {
                            mem[++adr] = OperationArray.op[opr].code;
                        }
                        catch (Exception ex)
                        {
                            Error.Opcode(i + 1);
                            return;
                        }
                    }
                    else
                    {
                        opr = term.Dequeue();
                        if (opr[0].isHEX())
                            mem[++adr] = opr[0];
                        else
                        {
                            Error.OprInvalid(i + 1);
                            return;
                        }
                    }
                }
                else
                {
                    if (term.Count != 0)
                    {
                        if (term.Dequeue()[0] != ';')
                        {
                            Error.OprMany(i + 1);
                            return;
                        }
                    }
                }
            }

            //結果を出力
            setMemText();
            setStatus();
        }
    }
}
