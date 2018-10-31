using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicographicPermutations
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 *  first gotta generate all the permutations that can be made with the digits 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
			 *  probably smart to save each one as a string in an array
			 *  then sort the array
			 *  and then blammo get the 1000000th one
			 * 
			 */

			List<string> list = new List<string>();
			List<string> store = new List<string>();

			long numberOfPermutations = 10 * 9 * 8 * 7 * 6 * 5 * 4 * 3 * 2 * 1;

			
			

			List<string> digits = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			List<string> test = new List<string> { "1", "2", "3"};
			List<string> array = perm(digits);

			array.Sort();
			Console.WriteLine(array[999999]);
			Console.ReadKey();
		}

		static List<string> perm(List<string> data)
		{

			List<string> result = new List<string>();
			List<string> tmp = new List<string>();
			List<string> tmp2 = new List<string>();
			List<string> tmp3 = new List<string>();
			//result = null;

			if (data.Count == 2)
			{
				result.Add(String.Concat(data[0], data[1]));
				result.Add(String.Concat(data[1], data[0]));
				return result;
			}
			else
			{	
				foreach(string s in data)
				{
					tmp.Clear();
					foreach (string n in data)
						tmp.Add(n);

					tmp.Remove(s);

					tmp2 = perm(tmp);
					foreach (string x in tmp2)
						tmp3.Add(String.Concat(s, x));
				}
			}

			foreach (string s in tmp3)
			{
				result.Add(s);
			}

			return result;
		}
	}
}
