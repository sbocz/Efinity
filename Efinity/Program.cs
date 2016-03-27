using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efinity
{
	class Program
	{
		static void Main(string[] args)
		{
			Database db = new Database();
			string applicationProvided;
			
			Console.WriteLine("Efinity Launched.");
			Console.WriteLine("Enter an application name to run:");
			applicationProvided = Console.ReadLine();


			Console.WriteLine("Getting \"" + applicationProvided + "\"s hash value..." );
			string hashValue = Hash.ComputeHash(applicationProvided);

			Console.WriteLine("Checking database for \"" + applicationProvided + 
				"\" with \"" + applicationProvided + "\"s hash value...");
			foreach (DataRow row in db.Application.Rows)
			{
				if((string)row["ApplicationName"] == applicationProvided && Hash.Confirm(applicationProvided, (string)row["HashKey"]))
					Console.WriteLine("Found!");
			}
			Console.ReadLine();
		}
	}
}
