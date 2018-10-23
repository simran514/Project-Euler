using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummationOfPrimes
{
	class Program
	{
		static void Main(string[] args)
		{
			long max = 2000;
			long p = 2;
			long sum = 0;
			long index = 0;

			List<long> array = new List<long>();

			for (int i = 1; i <= max; i++)
			{
				array.Add(i);
			}

			while (true)
			{
				while (p < max)
				{
					array.Remove(p);
					p *= 2;
				}

				for (long i = p; i <= max; i++)
				{
					if (array.Contains(i))
					{
						p = array.
					}
				}

			}

			for (long i = 0; i < max; i++)
			{
				if (IsPrime(i))
				{
					sum += i;
					Console.WriteLine(sum);
				}
			}

			Console.WriteLine(sum);
			Console.ReadKey();
		}

		static bool IsPrime(long num)
		{
			int count = 0;
			for (long i = 1; i <= num; i++)
			{
				if (num % i == 0)
				{
					count++;
					if (!(i == 1 || i == num))
					{
						count = 0;
						break;
					}
				}

			}

			if (count == 2)
				return true;
			else
				return false;
		}
	}
}
