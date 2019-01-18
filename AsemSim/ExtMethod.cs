﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsemSim
{
    public static class CharExt
    {
        public static int ToInt(this char self)
        {
            if ('0' <= self && self <= '9')
                return self - '0';
            else if ('A' <= self && self <= 'F')
                return self - 'A' + 10;
            return -1;
        }

        public static int ToChar(this int self)
        {
            if (0 <= self && self <= 9)
                return self + '0';
            else if (10 <= self && self <= 15)
                return self + 'A' - 10;
            return -1;
        }
    }
}
