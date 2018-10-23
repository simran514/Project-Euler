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
			long max = 2000000;
			long p = 2;
			long sum = 0;
			long index = 0;

			HashSet<long> array = new HashSet<long>();
			HashSet<long> primeArray = new HashSet<long>();

			HashSet<bool> boolArray = new HashSet<bool>();

			// populate the hashset with values from 2 to the max val
			for (long i = 2; i < max; i++)
			{
				array.Add(i);
			}

			//
			for (long i = 2; i < Math.Sqrt(max); i++)
			{
				for (long j = 2 * i; j < max; j += i)
				{
					array.Remove(j);
				}
			}

			foreach(long val in array)
			{
				sum += val;
				Console.WriteLine(sum);
			}

			Console.ReadKey();

		}

	}
}
