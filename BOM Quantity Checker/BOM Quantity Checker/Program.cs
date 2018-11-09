using HtmlAgilityPack;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using CsvHelper;
using Vse.Web.Serialization;
using WebApiContrib.Formatting;
using System.Web.Script.Serialization;



namespace BOM_Quantity_Checker
{
	class Program
	{
		
		[STAThread]
		static void Main(string[] args)
		{
			//var bom = new ExcelQueryFactory(@"C:\Users\SIMRA\Downloads\Replay Module Interface 1.2D BOM.xlsx");
			var bom = new ExcelQueryFactory(@"C:\Users\simran\source\repos\Project-Euler\BOM Quantity Checker\BOM Quantity Checker\bin\Debug\Replay Module Interface 1.2D BOM.xlsx");
			int boardQty = 10;
			string apiKey = "7a0bccc5";
			string octopartUrlBase = "http://octopart.com/api/v3";
			string octopartUrlEndpoint = "parts/match";
			List<Dictionary<string, string>> line_items = new List<Dictionary<string, string>>();
			List<Dictionary<string, string>> queries = new List<Dictionary<string, string>>();

			string[,] bomArray = BomToArray(bom);
			string supplier = "";

			for (int i = 1; i < bomArray.GetLength(0); i++)
			{
				if (bomArray[i, 8].Contains("digikey"))
					supplier = "digikey";
				if (bomArray[i, 8].Contains("mouser"))
					supplier = "mouser";

				line_items.Add(new Dictionary<string, string>()
				{
					{"line item", "line " + bomArray[i, 0]},
					{"manufacturer", bomArray[i, 1]},
					{"mpn", bomArray[i, 2]},
					{"footprint", bomArray[i, 7]},
					{"supplier", supplier}
				});
				
				queries.Add(new Dictionary<string, string>()
				{
					{"mpn", bomArray[i, 2]},
					{"brand", bomArray[i, 1]},
					{"reference",  "line " + bomArray[i, 0]}
				}
				);
			}

			string queryString = (new JavaScriptSerializer()).Serialize(line_items);


			string octopartUrlEntpoint = "parts/search";
			// Create the search request
			string query = "ERJ-2RKF1200X";
			var client = new RestClient(octopartUrlBase);
			var req = new RestRequest(octopartUrlEntpoint, Method.GET)
						.AddParameter("apikey", apiKey)
						.AddParameter("q", query)
						.AddParameter("start", "0")
						.AddParameter("limit", "10")
						.AddParameter("filter[fields][seller][]", "Digi-Key"); //this filter line dont work
			// Perform the search and obtain results
			var data = client.Execute(req).Content;
			var response = JsonConvert.DeserializeObject<dynamic>(data);

			//Console.WriteLine(response);
			
			Console.WriteLine();
			for (int i = 0; i < response.results[0].item.offers.Count; i++)
				Console.WriteLine(response.results[0].item.offers[i].seller.name);

			foreach(var result in response["results"])
			{
				var part = result["item"];
				//Console.WriteLine(part["brand"]["name"] + " - " + part["mpn"] + " - " + part["brand"]["homepage_url"]);
				Console.WriteLine(part["brand"]["name"]);
				Console.WriteLine(part["mpn"]);
				//Console.WriteLine(part["seller"]);

				for (int i = 0; i < part["offers"].Count; i++)
					Console.WriteLine(part["offers"][i]["seller"]["name"] + " - " + part["offers"][i]["in_stock_quantity"]);
					//Console.WriteLine(part["offers"][3]["seller"]["name"]);
				//Console.WriteLine(part["short_description"]);
				//Console.WriteLine(part["partoffer"]["in_stock_quantity"]);
				//Console.WriteLine(part["sku"]);


				Console.WriteLine();
			}
			Console.ReadKey();

			List<dynamic> results = new List<dynamic>();
			for (int i = 0; i < queries.Count; i += 20)
			{
				// Batch queries in groups of 20, query limit of
				// parts match endpoint
				var batched_queries = queries.GetRange(i, Math.Min(20, queries.Count - i));

				
				results.AddRange(response["results"]);
			}


			bool octopartApi = true;
			while (!octopartApi)
			{
				///*
				// * Ask user for number of boards needed
				// * Take that (number+1), multiply by the qty of each individual parts to get total amount of parts ( +1 to account for shit getting dropped or lost)
				// * Open the linked URL to see the qty available, do a compare
				// * Return the item #s that don't have enough stock available
				// * 
				// * If a part description says  DNM, then skip that for the comparison
				// * If the link is not for mouser or digikey, then throw that in a seperate "manual check" list
				// * 
				// */

				////Use when working on PC
				////var bom = new ExcelQueryFactory(@"C:\Users\simran\source\repos\Project-Euler\BOM Quantity Checker\BOM Quantity Checker\bin\Debug\Replay Module Interface 1.2D BOM.xlsx");
				////int boardQty = 10;

				////Use when working on surface
				//var bom = new ExcelQueryFactory(@"C:\Users\SIMRA\Downloads\Replay Module Interface 1.2D BOM.xlsx");
				//int boardQty = 10;

				////Console.WriteLine("Press ENTER to open file dialog to choose your BOM:");
				////Console.ReadKey();
				////var bom = new ExcelQueryFactory();
				////OpenFileDialog OFD = new OpenFileDialog();
				////OFD.Multiselect = false;
				////OFD.Title = "Open Excel Document";
				////OFD.Filter = "Excel Document|*.xlsx;*.xls";
				////OFD.ShowDialog();
				////string filePath = OFD.FileName;
				////bom.FileName = filePath.ToString();
				////Console.Write("Thanks! Please enter the quantity of boards needed: ");
				////int numBoards = Convert.ToInt32((Console.ReadLine()));
				////Console.WriteLine();
				////int boardQty = numBoards;


				//var worksheetNames = bom.GetWorksheetNames();
				//var columnNames = bom.GetColumnNames(worksheetNames.ElementAt(0));

				//List<string> itemNums = new List<string>();
				//List<string> colNames = new List<string>();

				//foreach (var c in columnNames)
				//{
				//	colNames.Add(c.ToString());
				//}

				//foreach (var c in bom.Worksheet())
				//{
				//	if (c[0].ToString() == "")
				//	{
				//		break;
				//	}

				//	itemNums.Add(c[0].ToString());
				//}

				//String[,] bomArray = new string[itemNums.Count() + 1, colNames.Count()];

				//for (int i = 0; i < colNames.Count; i++)
				//{
				//	bomArray[0, i] = colNames.ElementAt(i);
				//}

				//int bomRows = bomArray.GetLength(0);
				//int bomCols = bomArray.GetLength(1);

				///*
				// * Go through each item c in the worksheet
				// * c contains each element of each row
				// * 
				// * only increment col til we get to the end of the row
				// * take bomArray[row, col] == to c[col]
				// * 
				// * now go on to the next row, repeat til done
				// */
				//int row = 1;
				//foreach (var c in bom.Worksheet())
				//{
				//	for (int col = 0; col < bomCols; col++)
				//	{
				//		bomArray[row, col] = c[col];
				//	}
				//	row++;

				//	if (row == bomRows)
				//	{
				//		break;
				//	}
				//}

				////bomArray[i, 8] is gonna have the url we need
				////bomArray[i, 4] is gonna have the qty
				//List<string> itemOutOfStock = new List<string>();
				//List<string> itemNeedsManualCheck = new List<string>();

				//int availableStock = 0;
				//for (int i = 1; i < bomRows; i++)
				//{
				//	if (bomArray[i, 8].Contains("digikey"))
				//	{
				//		availableStock = GetDigikeyQty(bomArray[i, 8]);
				//		if (Convert.ToInt32(bomArray[i, 4]) * boardQty > availableStock)
				//		{
				//			itemOutOfStock.Add(bomArray[i, 0]);
				//		}
				//	}
				//	else if (bomArray[i, 8].Contains("mouser"))
				//	//if (bomArray[i, 8].Contains("mouser"))
				//	{
				//		availableStock = GetMouserQty(bomArray[i, 8]);
				//		if (Convert.ToInt32(bomArray[i, 4]) * boardQty > availableStock)
				//		{
				//			itemOutOfStock.Add(bomArray[i, 0]);
				//		}
				//	}
				//	else //needs to be manually checked
				//	{
				//		if (!bomArray[i, 3].Equals("DNM"))
				//			itemNeedsManualCheck.Add(bomArray[i, 0]);
				//	}
				//	availableStock = 0;
				//}


				//Console.WriteLine("The following items do not have enough stock available: ");
				//foreach (string s in itemOutOfStock)
				//{
				//	Console.WriteLine("Item " + s);
				//}

				//Console.WriteLine();

				//if (!(itemNeedsManualCheck.Count < 1))
				//	Console.WriteLine("The following items need to be checked manually: ");
				//foreach (string s in itemNeedsManualCheck)
				//{
				//	Console.WriteLine("Item " + s);
				//}

				//Console.ReadKey();
			}


		}

		static string[,] BomToArray(ExcelQueryFactory bom)
		{
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
			return bomArray;
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

		//Looks like Mouser.com is only allowing me to access their web page a set number of times within a certain time range?
		//After attempting to scrape web page after a set number of attemtps, all further access attempts return null values
		//SOLUTION: only use digikey links?
		static int GetMouserQty(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc = web.Load(url);

			var htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"pdpMainContentDiv\"]");
			var test = doc.DocumentNode.SelectNodes("h4[text()='In Stock']");

			string qty = doc.DocumentNode.SelectNodes("//*[@id=\"pdpPricingAvailability\"]/div[2]/div[1]/div[1]/div[2]/div/text()")[0].InnerText;

			for (int i = 0; i < 48; i++)
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
