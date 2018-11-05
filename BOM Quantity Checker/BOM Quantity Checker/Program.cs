using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqToExcel;


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
					break;
				itemNums.Add(c[0].ToString());
			}

			String[,] bomArray = new string[itemNums.Count()+1, colNames.Count()];

			for(int i = 0; i < colNames.Count; i++)
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
					break;
			}

			//var x = from c in bom.Worksheet()
			//		where c[0].Cast<int>() > 0
			//		select c;

			Console.ReadKey();
		}
	}
}
