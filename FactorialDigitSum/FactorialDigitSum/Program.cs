using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FactorialDigitSum
{
	class Program
	{
		static void Main(string[] args)
		{
			BigInteger num = factorial(100);
			long sum = 0;

			string numString = Convert.ToString(num);
			
			for (int i = 0; i < numString.Length; i++)
			{
				sum += (long)Char.GetNumericValue(numString[i]);
			}

			Console.WriteLine(sum);
			Console.ReadKey();
		}

		static BigInteger factorial(int n)
		{
			BigInteger tmp = 1;

			for (int i = n; i > 0; i--)
			{
				tmp *= i;
			}

			return tmp;
		}
	}
}
