using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bom_quote
{
	using CsvHelper;
	using Newtonsoft.Json;
	using RestSharp;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	using System.Web.Script.Serialization;

	

	public static class BomQuote
	{
		static void Main(string[] args)
		{
			BomQuote.ExecuteSearch();
		}

		public static void ExecuteSearch()
		{
			// Section 1
			// Convert CSV file into line items and queries
			// This code assumes a file format similar to the one on the
			// Arduino BOM
			List<Dictionary<string, string>> line_items = new List<Dictionary<string, string>>();
			List<Dictionary<string, string>> queries = new List<Dictionary<string, string>>();

			//StreamReader reader = File.OpenText("arduino_bom.csv");
			//var csv = new CsvReader(reader);
			
			//while (csv.Read())
			//{
			//	csv.ReadHeader();
			//	var mpn = csv.GetField<string>("Part Number");
			//	var brand = csv.GetField<string>("Manufacturer");
			//	if (!string.IsNullOrEmpty(mpn) && !string.IsNullOrEmpty(brand))
			//	{
			//		line_items.Add(Enumerable.Range(0, csv.Context.HeaderRecord.Length).ToDictionary(i => csv.Context.HeaderRecord[i], i => csv.CurrentRecord[i]));
			//		queries.Add(new Dictionary<string, string> {
			//			{"mpn", mpn},
			//			{"brand", brand},
			//			{"reference", (line_items.Count - 1).ToString()}
			//		});
			//	}
			//}

			queries.Add(new Dictionary<string, string> {
						{"mpn", "CC0603KRX7R9BB104"},
						{"brand", "Yageo"},
						{"reference", "1"}
					});
			


			// Section 2
			// Send queries to REST API for part matching
			List<dynamic> results = new List<dynamic>();
			for (int i = 0; i < queries.Count; i += 20)
			{
				// Batch queries in groups of 20, query limit of
				// parts match endpoint
				var batched_queries = queries.GetRange(i, Math.Min(20, queries.Count - i));

				string octopartUrlBase = "http://octopart.com/api/v3";  // Octopart API url
				string octopartUrlEntpoint = "parts/match";             // Octopart search type
				string apiKey = APIKEY;                             // -- your API key -- (https://octopart.com/api/register)

				// Create the search request
				string queryString = (new JavaScriptSerializer()).Serialize(batched_queries);
				var client = new RestClient(octopartUrlBase);
				var req = new RestRequest(octopartUrlEntpoint, Method.GET)
							.AddParameter("apikey", apiKey)
							.AddParameter("queries", queryString);

				// Perform the search and obtain results
				var data = client.Execute(req).Content;
				var response = JsonConvert.DeserializeObject<dynamic>(data);
				results.AddRange(response["results"]);
			}


			// Section 3
			// Analyze results sent back Octopart API
			Console.WriteLine("Found " + line_items.Count + " line items in BOM.");
			// Price BOM
			int hits = 0;
			double total_avg_price = 0.0;
			foreach (var result in results)
			{
				//var line_item = line_items[(int)result["reference"]];
				if (!result["items"].HasValues)
				{
				//	Console.WriteLine(String.Format("Did not find a match on line item #{0} ({1})", (int)result["reference"] + 1, string.Join(" ", line_item.Values.ToArray<string>())));
					continue;
				}

				// Get pricing from the first item for desired quantity
				//int quantity = int.Parse(line_item["Qty"]);
				List<double> prices = new List<double>();
				foreach (var offer in result["items"][0]["offers"])
				{
					if (offer["prices"]["USD"] == null)
						continue;
					double price = 0;
					foreach (var price_tuple in offer["prices"]["USD"])
					{
						// Find correct price break
						if (price_tuple[0] > 10)
							break;
						price = price_tuple[1];
					}
					if (price != 0)
					{
						prices.Add(price);
					}
				}

				if (prices.Count == 0)
				{
				//	Console.WriteLine(String.Format("Did not find USD pricing on line item #{0} ({1})", (int)result["reference"] + 1, string.Join(" ", line_item.Values.ToArray<string>())));
					continue;
				}

				double avg_price = 10 * prices.Sum() / prices.Count;
				total_avg_price += avg_price;
				hits++;
			}

			Console.WriteLine(String.Format("Matched on {0:0.0}% of BOM, total average prices is USD {1:0.00}.", (hits / (float)line_items.Count) * 100, total_avg_price));
		}

		private const string APIKEY = "7a0bccc5";
	}
}

