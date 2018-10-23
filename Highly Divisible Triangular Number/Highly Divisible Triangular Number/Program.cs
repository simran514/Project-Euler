using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highly_Divisible_Triangular_Number
{
	class Program
	{
		static void Main(string[] args)
		{
			long x = 1;
			long y = 0;
			long tmp = 0;
			long max = 0;
			HashSet<long> array = new HashSet<long>();

			while(true)
			{
				y = triangularNumber(x);
				array = divisors(y);

				tmp = array.Count;
				if (tmp > max)
				{
					max = tmp;
					Console.WriteLine(max);
				}

				if (array.Count > 500)
				{
					Console.WriteLine((int)y);
					Console.ReadKey();
				}
				x++;

				array.Clear();
			}

			Console.ReadKey();
		}

		static long triangularNumber(long n)
		{
			long tmp = 0;

			for (int i = 1; i <= n; i++)
				tmp += i;

			return tmp;
		}

		static HashSet<long> divisors(long n)
		{
			HashSet<long> tmp = new HashSet<long>();

			for (int i = 1; i <= Math.Sqrt(n); i++)
			{
				if (n % i == 0)
				{
					tmp.Add(i);
					if (i != n / i)
						tmp.Add(n / i);
				}
			}

			return tmp;
		}
	}

}
