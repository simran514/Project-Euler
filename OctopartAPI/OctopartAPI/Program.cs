using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopartAPI
{
	using Newtonsoft.Json;
	using RestSharp;
	using System;
	using System.Collections.Generic;
	using System.Web.Script.Serialization;
	using Vse.Web.Serialization;

	class Program
	{
		static void Main(string[] args)
		{
			PartsMatch.ExecuteSearch();
			Console.ReadKey();
		}
	}

	public static class PartsMatch
	{
		public static void ExecuteSearch()
		{
			// -- your search query --
			var query = new List<dynamic>()
			{
				new Dictionary<string, string>()
				{ { "mpn", "SN74S74N" },
				  { "reference", "line1" } },
				new Dictionary<string, string>()
				{ { "sku", "67K1122" },
				  { "reference", "line2" } },
				new Dictionary<string, string>()
				{ { "mpn_or_sku", "SN74S74N" },
				  { "reference", "line3" } },
				new Dictionary<string, string>()
				{ { "brand", "Texas Instruments" },
				  { "mpn", "SN74S74N" },
				  { "reference", "line4" } }
			};

			string octopartUrlBase = "http://octopart.com/api/v3";
			string octopartUrlEndpoint = "parts/match";
			string apiKey = APIKEY;

			// Create the search request
			string queryString = (new JavaScriptSerializer()).Serialize(query);
			var client = new RestClient(octopartUrlBase);
			var req = new RestRequest(octopartUrlEndpoint, Method.GET)
						.AddParameter("apikey", apiKey)
						.AddParameter("queries", queryString);

			// Perform the search and obtain results
			var data = client.Execute(req).Content;
			var response = JsonConvert.DeserializeObject<dynamic>(data);

			// Print request time (in milliseconds)
			Console.WriteLine(response["msec"]);

			// Print mpn's
			foreach (var result in response["results"])
			{
				Console.WriteLine("Reference: " + result["reference"]);
				foreach (var item in result["items"])
				{
					Console.WriteLine(item["mpn"]);
					
					
					
				}
			}
		}

		// -- your API key -- (https://octopart.com/api/register)
		private const string APIKEY = "7a0bccc5";
	}
}
