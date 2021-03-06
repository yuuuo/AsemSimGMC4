﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace AsemSim
{
	public partial class Form1 : Form
	{
		/// <summary>
		/// 実行ボタンイベント
		/// </summary>
		private void buttonRun_Click(object sender, EventArgs e)
		{
			timer1.Start();
			address = 0;
			exFlag = true;
		}

		/// <summary>
		/// リセットボタンイベント
		/// </summary>
		private void buttonReset_Click(object sender, EventArgs e)
		{
			setStatus();
			timer1.Stop();
		}

		/// <summary>
		/// タイマー割り込み
		/// シミュレーター実行部
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (address > 97)
			{
				timer1.Stop();
				return;
			}
			CodeExecution();
			setStatus();
		}

		/// <summary>
		/// 命令実行部
		/// </summary>
		private void CodeExecution()
		{
			int tmp;
			switch (mem[address])
			{
				case '0':
					if (key != -1)
					{
						ar = key;
						exFlag = false;
					}
					else
					{
						exFlag = true;
					}
					address++;
					break;
				case '1':
					SetSevenLED(ar);
					address++;
					exFlag = true;
					break;
				case '2':
					//tmp : swap
					tmp = ar;
					ar = br;
					br = tmp;
					tmp = yr;
					zr = yr;
					zr = tmp;
					address++;
					exFlag = true;
					break;
				case '3':
					//tmp : swap
					tmp = ar;
					ar = yr;
					yr = tmp;
					address++;
					exFlag = true;
					break;
				case '4':
					dm[yr] = ar;
					address++;
					exFlag = true;
					break;
				case '5':
					ar = dm[yr];
					Console.WriteLine(yr);
					Console.WriteLine(dm[yr]);
					address++;
					exFlag = true;
					break;
				case '6':
					ar = (ar + dm[yr]) % 16;
					exFlag = (ar + dm[yr]) / 16 > 0;
					address++;
					break;
				case '7':
					//tmp : 引き算の結果 負なら : 実行フラグtrue & 0~15に変換
					ar = (exFlag = (tmp = dm[yr] - ar) < 0) ? 16 + tmp : tmp;
					address++;
					break;
				case '8':
					ar = mem[address + 1].ToInt();
					address += 2;
					exFlag = true;
					break;
				case '9':
					ar = (ar + mem[address + 1].ToInt()) % 16;
					exFlag = (ar + mem[address + 1].ToInt()) / 16 > 0;
					address += 2;
					break;
				case 'A':
					yr = mem[address + 1].ToInt();
					address += 2;
					exFlag = true;
					break;
				case 'B':
					yr = (yr + mem[address + 1].ToInt()) % 16;
					exFlag = (yr + mem[address + 1].ToInt()) / 16 > 0;
					address += 2;
					break;
				case 'C':
					exFlag = ar != mem[address + 1].ToInt();
					address += 2;
					break;
				case 'D':
					exFlag = yr != mem[address + 1].ToInt();
					address += 2;
					break;
				case 'F':
					if (exFlag)
					{
						address = int.Parse(new string(new char[2] { mem[address + 1], mem[address + 2] }), NumberStyles.AllowHexSpecifier);
					}
					else
					{
						address += 3;
						exFlag = true;
					}
					break;
				case 'E':
					// 2バイト命令
					address++;
					ECodeExecution();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// サブルーチン2バイト命令を実行する関数
		/// </summary>
		private void ECodeExecution()
		{
			int[] SoundFreq = new int[] { 0, 220, 262, 297, 339, 349, 392, 440, 494, 523, 587, 659, 698, 784, 0 };
			switch (mem[address])
			{
				case '0':
					SetSevenLED(-1);
					address++;
					exFlag = true;
					break;
				case '1':
					SetBinaryLED(true, yr);
					address++;
					exFlag = true;
					break;
				case '2':
					SetBinaryLED(false, yr);
					address++;
					exFlag = true;
					break;
				case '4':
					ar ^= 0xF;
					address++;
					exFlag = true;
					break;
				case '5':
					//tmp : swap
					int tmp;
					tmp = ar;
					ar = ar_;
					ar_ = tmp;

					tmp = br;
					br = br_;
					br_ = tmp;

					tmp = yr;
					yr = yr_;
					yr_ = tmp;

					tmp = zr;
					zr = zr_;
					zr_ = tmp;
					address++;
					exFlag = true;
					break;
				case '6':
					exFlag = ar % 2 == 0;
					ar >>= 1;
					address++;
					break;
				case '7':
					try
					{
						SoundPlayer end = new SoundPlayer(@"sound\end.wav");
						end.Play();
					}
					catch (Exception ex)
					{
						Error.SoundNotFound();
					}

					exFlag = true;
					break;
				case '8':
					try
					{
						SoundPlayer error = new SoundPlayer(@"sound\error.wav");
						error.Play();
					}
					catch (Exception ex)
					{
						Error.SoundNotFound();
					}
					exFlag = true;
					break;
				case '9':
					try
					{
						SoundPlayer shorts = new SoundPlayer(@"sound\short.wav");
						shorts.Play();
					}
					catch (Exception ex)
					{
						Error.SoundNotFound();
					}
					exFlag = true;
					break;
				case 'A':
					try
					{
						SoundPlayer longs = new SoundPlayer(@"sound\long.wav");
						longs.Play();
					}
					catch (Exception ex)
					{
						Error.SoundNotFound();
					}
					exFlag = true;
					break;
				case 'B':
					Console.Beep(SoundFreq[ar], 1000);
					exFlag = true;
					break;
				case 'C':
					// Intervalによって影響を受ける 1.0 / (timer1.Interval / 100.0)
					address--;
					if (waitTimer == -1)
					{
						waitTimer = ar + 1;
					}
					else
					{
						waitTimer--;
						if (waitTimer == 0)
						{
							waitTimer = -1;
							address += 2;
						}
					}
					exFlag = true;
					break;
				case 'D':
					SetBinaryLED(dm[0x7] << 4 | dm[0xF]);
					address++;
					exFlag = true;
					break;
				case 'E':
					exFlag = true;
					break;
				case 'F':
					exFlag = true;
					break;
				default:
					break;
			}
		}
	}
}
