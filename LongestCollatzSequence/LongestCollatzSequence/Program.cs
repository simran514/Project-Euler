using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestCollatzSequence
{
	class Program
	{
		static void Main(string[] args)
		{
			long bestNum = 0;
			long bestLength = 0;
			long tmp = 0;
			long max = 1000000;

			//collatz(13);
			
			for (long i = 1; i <= max; i++)
			{
				tmp = collatz(i);

				if (tmp > bestLength)
				{
					bestLength = tmp;
					bestNum = i;

					

				}
			}


			collatz(13);

			//2.674 s
			Console.WriteLine("Best Number is " + bestNum + " with " + bestLength + " terms.");
			Console.ReadKey();

		}

		static long collatz(long n)
		{
			long count = 0;
			
				if (n == 1)
				{
					//	Console.Write(n);
					count = 1;
				}
				else if (n % 2 == 0)
				{
					//	Console.Write(n + " -> ");
					//n = n / 2;

					count = 1 + collatz(n / 2);
				}
				else
				{
					//	Console.Write(n + " -> ");
					//n = 3 * n + 1;
					count = 2 + collatz((3 * n + 1) / 2);

				}
			

			//Console.WriteLine(" : Count is " + count);
			return count;
		}
	}
}
