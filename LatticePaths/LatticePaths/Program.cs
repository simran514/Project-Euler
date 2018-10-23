using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace LatticePaths
{
	class Program
	{
		static void Main(string[] args)
		{
			
			Console.WriteLine(lattice(20, 20));
			Console.ReadKey();
		}

		static BigInteger lattice(int n, int m)
		{

			/*
			 * I can either increase the x, or the y up until the limit
			 * and then after each increase, i again have the same choices available to me up until the limits again
			 * and i gotta keep doing this until i reach the limit of both numbers
			 * 
			 * 
			 */
			return factorial(n + m) / (factorial(m)*factorial(n));			

		}

		static BigInteger factorial(long n)
		{
			BigInteger tmp = 1 ;
			for (int i = 1; i <= n; i++)
			{
				tmp *= i;
			}

			return tmp;
		}
	}
}
