using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaximumPathSumI
{
	class Program
	{
		static void Main(string[] args)
		{
			string data =     "75"
						   + " 95 64"
						   + " 17 47 82"
						   + " 18 35 87 10"
						   + " 20 04 82 47 65"
						   + " 19 01 23 75 03 34"
						   + " 88 02 77 73 07 63 67"
						   + " 99 65 04 28 06 16 70 92"
						   + " 41 41 26 56 83 40 80 70 33"
						   + " 41 48 72 33 47 32 37 16 94 29"
						   + " 53 71 44 65 25 43 91 52 97 51 14"
						   + " 70 11 33 28 77 73 17 78 39 68 17 57"
						   + " 91 71 52 38 17 14 91 43 58 50 27 29 48"
						   + " 63 66 04 68 89 53 67 30 73 16 69 87 40 31"
						   + " 04 62 98 27 23 09 70 98 73 93 38 53 60 04 23";

			string dataTmp =    "3"
							 + " 7 4"
							 + " 2 4 6"
							 + " 8 5 9 3";

			String[] stringArray = data.Split(' ');
			int[] numArray = new int[stringArray.Length];
			int numOfLevels;
			HashSet<int> currentLevel = new HashSet<int>();
			int sum = 0;
			



			for (int i = 0; i < stringArray.Length; i++)
			{
				numArray[i] = Convert.ToInt32(stringArray[i]);
			}

			numOfLevels = levels(numArray);

			int a;
			int b;
			int c;
			a = 0;
			b = 0;
			c = 0;
			int index = 0;

			for (int i = numOfLevels - 1; i >= 1; i--)
			{
				/*
				 * numArray[14.1 ] += Math.Max(numArray[15.1], numArray[15.2]);
				 * 
				 */
				 for (int j = 0; j < i; j++)
				{
					index = firstIndexOfLevel(i)-1;
					a = numArray[firstIndexOfLevel(i) + j -1];
					b = numArray[firstIndexOfLevel(i + 1) + j -1];
					c = numArray[firstIndexOfLevel(i + 1) + j ];
					numArray[firstIndexOfLevel(i) + j -1] += Math.Max(numArray[firstIndexOfLevel(i + 1) + j -1], numArray[firstIndexOfLevel(i + 1) + j]);
				}
				
			}

			Console.WriteLine(numArray[0]);
			Console.ReadKey();


		}

		static int firstIndexOfLevel(int level)
		{
			int tmp = 0;

			for (int i = level-1; i > 0; i--) 
				tmp += i;

			return tmp + 1;
		}

		static int levels(int[] x)
		{
			int count = 0;
			int tmp = x.Length;

			for (int i = 1; i <= x.Length; i++)
			{
				if (tmp - i == 0)
				{
					count++;
					break;
				}

				tmp -= i;
				count++;
			}
			return count;
		}
	}
}
