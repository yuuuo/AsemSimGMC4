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
        public static void Start()
        {
            ShowMessage("START is not Found");
        }

        public static void End()
        {
            ShowMessage("END is not Found");
        }

		public static void Opcode(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Operation is not Found");
		}

		public static void OprInvalid(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Operand is Invalid");
		}

		public static void OprMany(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Too many Operands");
		}

		public static void LabelMany(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Label is Already Used");
		}

		public static void LabelNotFound(int l)
		{
			ShowMessage("L:" + l.ToString() + "  Label is not Found");
		}

		private static void ShowMessage(string str)
        {
            MessageBox.Show(str, "Assemble Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
