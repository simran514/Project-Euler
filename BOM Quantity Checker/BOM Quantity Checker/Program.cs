using HtmlAgilityPack;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BOM_Quantity_Checker
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			/*
			 * Ask user for number of boards needed
			 * Take that (number+1), multiply by the qty of each individual parts to get total amount of parts ( +1 to account for shit getting dropped or lost)
			 * Open the linked URL to see the qty available, do a compare
			 * Return the item #s that don't have enough stock available
			 * 
			 * If a part description says  DNM, then skip that for the comparison
			 * If the link is not for mouser or digikey, then throw that in a seperate "manual check" list
			 * 
			 */

			//Console.WriteLine("Press ENTER to open file dialog to choose your BOM:");
			//Console.ReadKey();

			var bom = new ExcelQueryFactory(@"C:\Users\simran\source\repos\Project-Euler\BOM Quantity Checker\BOM Quantity Checker\bin\Debug\Replay Module Interface 1.2D BOM.xlsx");
			//OpenFileDialog OFD = new OpenFileDialog();
			//OFD.Multiselect = false;
			//OFD.Title = "Open Excel Document";
			//OFD.Filter = "Excel Document|*.xlsx;*.xls";
			//OFD.ShowDialog();
			//string filePath = OFD.FileName;
			//bom.FileName = filePath.ToString();

			//Console.Write("Thanks! Please enter the quantity of boards needed: ");
			//int numBoards = Convert.ToInt32((Console.ReadLine()));
			//Console.WriteLine();

			int boardQty = 10;

			var worksheetNames = bom.GetWorksheetNames();
			var columnNames = bom.GetColumnNames(worksheetNames.ElementAt(0));

			List<string> itemNums = new List<string>();
			List<string> colNames = new List<string>();

			foreach (var c in columnNames)
			{
				colNames.Add(c.ToString());
			}

			foreach (var c in bom.Worksheet())
			{
				if (c[0].ToString() == "")
				{
					break;
				}

				itemNums.Add(c[0].ToString());
			}

			String[,] bomArray = new string[itemNums.Count() + 1, colNames.Count()];

			for (int i = 0; i < colNames.Count; i++)
			{
				bomArray[0, i] = colNames.ElementAt(i);
			}

			int bomRows = bomArray.GetLength(0);
			int bomCols = bomArray.GetLength(1);

			/*
			 * Go through each item c in the worksheet
			 * c contains each element of each row
			 * 
			 * only increment col til we get to the end of the row
			 * take bomArray[row, col] == to c[col]
			 * 
			 * now go on to the next row, repeat til done
			 */
			int row = 1;
			foreach (var c in bom.Worksheet())
			{
				for (int col = 0; col < bomCols; col++)
				{
					bomArray[row, col] = c[col];
				}
				row++;

				if (row == bomRows)
				{
					break;
				}
			}

			//bomArray[i, 8] is gonna have the url we need
			//bomArray[i, 4] is gonna have the qty
			List<string> itemOutOfStock = new List<string>();
			List<string> itemNeedsManualCheck = new List<string>();

			int availableStock = 0;
			for (int i = 1; i < bomRows; i++)
			{
				if (bomArray[i, 8].Contains("digikey"))
				{
					availableStock = GetDigikeyQty(bomArray[i, 8]);
					if (Convert.ToInt32(bomArray[i, 4]) * boardQty > availableStock)
					{
						itemOutOfStock.Add(bomArray[i, 0]);
					}
				}
				else if (bomArray[i, 8].Contains("mouser"))
				//if (bomArray[i, 8].Contains("mouser"))
				{
					availableStock = GetMouserQty(bomArray[i, 8]);
					if (Convert.ToInt32(bomArray[i, 4]) * boardQty > availableStock)
					{
						itemOutOfStock.Add(bomArray[i, 0]);
					}
				}
				else //needs to be manually checked
				{

				}
				availableStock = 0;
			}


			Console.WriteLine("The following items do not have enough stock available: ");
			foreach (string s in itemOutOfStock)
			{
				Console.WriteLine("Item " + s);
			}

			Console.ReadKey();
		}

		static int GetDigikeyQty(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc = web.Load(url);

			var htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"dkQty\"]");
			var test = doc.DocumentNode.SelectNodes("span[text()='10,000']");

			string qty = doc.DocumentNode.SelectNodes("//*[@id=\"dkQty\"]")[0].InnerText;

			for (int i = 32; i < 48; i++)
			{
				qty = qty.Replace(((char)i).ToString(), "");
			}

			for (int i = 58; i < 127; i++)
			{
				qty = qty.Replace(((char)i).ToString(), "");
			}
			return Convert.ToInt32(qty);
		}

		static int GetMouserQty(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc = web.Load(url);

			var htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"pdpMainContentDiv\"]");
			var test = doc.DocumentNode.SelectNodes("h4[text()='In Stock']");

			string qty = doc.DocumentNode.SelectNodes("//*[@id=\"pdpPricingAvailability\"]/div[2]/div[1]/div[1]/div[2]/div/text()")[0].InnerText;

			for (int i = 32; i < 48; i++)
			{
				qty = qty.Replace(((char)i).ToString(), "");
			}

			for (int i = 58; i < 127; i++)
			{
				qty = qty.Replace(((char)i).ToString(), "");
			}

			return Convert.ToInt32(qty);
		}


	}
}
