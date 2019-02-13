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

        private static void ShowMessage(string str)
        {
            MessageBox.Show(str, "Assemble Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
