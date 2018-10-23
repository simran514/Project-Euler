using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PowerDigitSum
{
	class Program
	{
		static void Main(string[] args)
		{
			BigInteger x = power(2, 15);
			BigInteger sum = sumOfDigits(x);

			Console.WriteLine(sumOfDigits(power(2, 1000)));
			Console.ReadKey();
		}

		static BigInteger power(int a, int x)
		{
			BigInteger tmp = new BigInteger(a);

			for (int i = 1; i < x; i++)
				tmp *= a;

			return tmp;
		}

		static BigInteger sumOfDigits(BigInteger x)
		{
			BigInteger tmp = 0;
			string tmpString = Convert.ToString(x);
			int[] tmpArray = new int[tmpString.Length];

			for (int i = 0; i < tmpString.Length; i++)
			{
				tmp += (BigInteger)Char.GetNumericValue(tmpString[i]);
			}

			return tmp;
		}
	}
}
