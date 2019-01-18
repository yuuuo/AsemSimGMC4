using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsemSim
{
    public static class OperationArray
    {
        /// <summary>
        /// 命令ごとに命令コードと命令の長さを入れるクラス
        /// </summary>
        public class Operation
        {
            public char code;
            public int length;
            public Operation(char _code, int len)
            {
                code = _code;
                length = len;
            }
        }

        /// <summary>
        /// 命令から機械語に変換するためのテーブル
        /// </summary>
        public static Dictionary<string, Operation> op = new Dictionary<string, Operation>()
        {
            {"KA", new Operation('0', 1) },
            {"AO", new Operation('1', 1) },
            {"CH", new Operation('2', 1) },
            {"CY", new Operation('3', 1) },
            {"AM", new Operation('4', 1) },
            {"MA", new Operation('5', 1) },
            {"M+", new Operation('6', 1) },
            {"M-", new Operation('7', 1) },
            {"TIA", new Operation('8', 2) },
            {"AIA", new Operation('9', 2) },
            {"TIY", new Operation('A', 2) },
            {"AIY", new Operation('B', 2) },
            {"CIA", new Operation('C', 2) },
            {"CIY", new Operation('D', 2) },
            {"JUMP", new Operation('F', 3) },
            {"RET", new Operation('F', 1) },
            {"CAL", new Operation('E', 2) },

            {"RSTO", new Operation('0', 1) },
            {"SETR", new Operation('1', 1) },
            {"RSTR", new Operation('2', 1) },
            {"CMPL", new Operation('4', 1) },
            {"CHNG", new Operation('5', 1) },
            {"SIFT", new Operation('6', 1) },
            {"ENDS", new Operation('7', 1) },
            {"ERRS", new Operation('8', 1) },
            {"SHTS", new Operation('9', 1) },
            {"LONS", new Operation('A', 1) },
            {"SUND", new Operation('B', 1) },
            {"TIMR", new Operation('C', 1) },
            {"DSPR", new Operation('D', 1) },
            {"DEM-", new Operation('E',1) },
            {"DEM+", new Operation('F', 1) }

        };
    }
}
