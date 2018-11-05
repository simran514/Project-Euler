using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace TextUsageChart
{
	class Program
	{
		static void Main(string[] args)
		{
			string contents = File.ReadAllText(@"C:\Users\simran\AppData\Roaming\Apple Computer\MobileSync\Backup\5c34d9ae25cc0377b12843748fd1d277a14903fe\3d\3d0d7e5fb2ce288813306e4d4636395e047a3d28");
			DataClass txtData = new DataClass(contents);
			Console.WriteLine(contents);
			Console.ReadKey();
		}

	}

	class DataClass
	{
		private SQLiteConnection sqlite;

		public DataClass(string dataPath)
		{
			//This part killed me in the beginning.  I was specifying "DataSource"
			//instead of "Data Source"
			sqlite = new SQLiteConnection("Data Source=" + dataPath + ";New=False");

		}

		public DataTable selectQuery(string query)
		{
			SQLiteDataAdapter ad;
			DataTable dt = new DataTable();

			try
			{
				SQLiteCommand cmd;
				sqlite.Open();  //Initiate connection to the db
				cmd = sqlite.CreateCommand();
				cmd.CommandText = query;  //set the passed query
				ad = new SQLiteDataAdapter(cmd);
				ad.Fill(dt); //fill the datasource
			}
			catch (SQLiteException ex)
			{
				//Add your exception code here.
			}
			sqlite.Close();
			return dt;
		}
	}
}
