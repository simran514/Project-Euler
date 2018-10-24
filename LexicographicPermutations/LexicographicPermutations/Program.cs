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

			long numberOfPermutations = 9 * 8 * 7 * 6 * 5 * 4 * 3 * 2 * 1;


			List<string> digits = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			List<string> test = new List<string> { "1", "2"};
			List<string> array = perm(test);
			

		}

		static List<string> perm(List<string> data)
		{
			List<string> tmp = new List<string>();
			List<string> tmp2 = new List<string>();
			List<string> array = new List<string>();

			if (data.Count == 1)
			{
				tmp.Add(data[0]);
			}
			else
			{
				for (int i = 0; i < data.Count; i++)
				{
					/*
					 * tmp2 = data;
					 * tmp2 = tmp2.RemoveAt(i);
					 * 
					 * tmp.Add(data[i] + perm(tmp2));
					 */

					tmp2 = data;
					tmp2.RemoveAt(i);
					//Need to go through each value in tmp2 and concat them to data[i] separately before adding to tmp
					array = perm(tmp2);
					foreach(string s in array)
					tmp.Add(String.Concat(data[i] , s));
			
				}

			}


			return tmp;
		}
	}
}
