using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsemSim
{
    static class Error
    {
        /// <summary>
        /// STARTが見つからなかった
        /// </summary>
        public static void Start()
        {
            ShowMessage("START is not Found");
        }

        /// <summary>
        ////ENDが見つからなかった
        /// </summary>
        public static void End()
        {
            ShowMessage("END is not Found");
        }

        /// <summary>
        /// オペコードが見つからなかった
        /// </summary>
        /// <param name="l">行数</param>
		public static void Opcode(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Operation is not Found");
		}

        /// <summary>
        /// オペランド不正
        /// </summary>
        /// <param name="l">行数</param>
		public static void OprInvalid(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Operand is Invalid");
		}

        /// <summary>
        /// オペラントが多い
        /// </summary>
        /// <param name="l">行数</param>
		public static void OprMany(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Too many Operands");
		}

        /// <summary>
        /// ラベル重複
        /// </summary>
        /// <param name="l">行数</param>
		public static void LabelMany(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Label is Already Used");
		}

        /// <summary>
        /// ラベルが見つからない
        /// </summary>
        /// <param name="l">行数</param>
		public static void LabelNotFound(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Label is not Found");
		}

        /// <summary>
        /// サウンドファイルが見つからない
        /// </summary>
		public static void SoundNotFound()
		{
			ShowMessage("Sound File is not Found");
		}

        /// <summary>
        /// エラーメッセージボックスを表示
        /// </summary>
        /// <param name="str">文字列</param>
		private static void ShowMessage(string str)
        {
            MessageBox.Show(str, "Assemble Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
