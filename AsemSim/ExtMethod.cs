﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsemSim
{
    public static class CharExt
    {
        /// <summary>
        /// 拡張メソッド
        /// 16進char型をint型にキャスト
        /// </summary>
        /// <returns>-1でエラー</returns>
        public static int ToInt(this char self)
        {
            if ('0' <= self && self <= '9')
                return self - '0';
            else if ('A' <= self && self <= 'F')
                return self - 'A' + 10;
            return -1;
        }

        /// <summary>
        /// 拡張メソッド
        /// int型を16進charにキャスト
        /// </summary>
        /// <returns>255でエラー</returns>
        public static char ToChar(this int self)
        {
            if (0 <= self && self <= 9)
                return (char)(self + '0');
            else if (10 <= self && self <= 15)
                return (char)(self + 'A' - 10);
            return (char)255;
        }


        /// <summary>
        /// 拡張メソッド
        /// char型文字列が16進数か判定
        /// </summary>
        /// <param name="self">値</param>
        /// <returns>16進数</returns>
		public static bool isHEX(this char self)
		{
			return -1 != self.ToString().ToUpper()[0].ToInt();
		}
    }
}

