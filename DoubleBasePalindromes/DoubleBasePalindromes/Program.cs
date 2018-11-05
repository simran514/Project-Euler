using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBasePalindromes
{
	class Program
	{
		static void Main(string[] args)
		{
			int n = 119;
			Console.WriteLine(n);
			Console.WriteLine(IntIsPalindrome(n));

			Console.WriteLine(IntToBinary(n));
			Console.WriteLine(BinIsPalindrome(n));

			long sum = 0;
			for (int i = 0; i < 1000000; i++)
			{
				if (IntIsPalindrome(i) && BinIsPalindrome(i))
					sum += i;
			}

			Console.ReadKey();
		}

		static string IntToString(int n)
		{
			return Convert.ToString(n);
		}

		static bool IntIsPalindrome(int n)
		{
			string s = IntToString(n);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] != s[s.Length - 1 - i])
					return false;
			}
			return true;
		}

		static string IntToBinary(int n)
		{
			string s = Convert.ToString(n, 2);
			return Convert.ToString(n, 2);
		}

		static bool BinIsPalindrome(int n)
		{
			string s = IntToBinary(n);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] != s[s.Length - 1 - i])
					return false;
			}
			return true;
		}
	}
}
