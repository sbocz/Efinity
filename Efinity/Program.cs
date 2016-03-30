using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFinity
{
	class Program
	{
		static void Main(string[] args)
		{
			Database db = new Database();
			Console.WriteLine("Efinity Launched.");

			bool validDemo = false;
			int demoType = 0;
			while (!validDemo)
			{
				try
				{
					Console.WriteLine("Hash demo(1) or certificate demo(2)?");
					demoType = Convert.ToInt32(Console.ReadLine());
					validDemo = true;
				}
				catch (Exception)
				{
					validDemo = false;
				}
			}

			switch (demoType)
			{
				case 1:
					HashDemo(db);
					break;
				case 2:
					CertificateDemo(db);
					break;
			}
		}

		/// <summary>
		/// Demo for Efinity certificate system.
		/// </summary>
		/// <param name="db">Database to use.</param>
		private static void CertificateDemo(Database db)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Demo for Efinity hashing system.
		/// </summary>
		/// <param name="db">Database to use</param>
		private static void HashDemo(Database db)
		{
			bool running = true;

			//Repeat until user wishes to stop running apps
			while (running)
			{
				Console.WriteLine("\nEnter an application name to run:");
				var applicationProvided = Console.ReadLine();
				bool versionInt = false;
				int versionProvided = 0;

				//Make sure version is an integer
				while (!versionInt)
				{
					try
					{
						Console.WriteLine("Enter the application version:");
						versionProvided = Convert.ToInt32(Console.ReadLine());
						versionInt = true;
					}
					catch (Exception)
					{
						versionInt = false;
					}
				}

				//Check for application being supported
				Console.WriteLine("\nChecking database for \"" + applicationProvided + "\"...");
				bool appSupported = false;
				foreach (DataRow row in db.Application.Rows)
				{
					if ((string)row["ApplicationName"] == applicationProvided)
					{
						appSupported = true;
						break;
					}
				}

				if (appSupported)
				{
					Console.WriteLine("Application supported by App2020...");
					Console.WriteLine("\nGetting \"" + applicationProvided + "\"s hash value...");
					
					//Check for application + version being supported
					Console.WriteLine("Checking database for \"" + applicationProvided + "\"s hash value...\n");
					bool versionSupported = false;
					foreach (DataRow row in db.Version.Rows)
					{
						if ((string)row["ApplicationName"] == applicationProvided && (int)row["VersionNumber"] == versionProvided && Hash.Confirm(applicationProvided + versionProvided + "Data", (string)row["HashKey"]))
						{
							versionSupported = true;
							break;
						}
					}
					if (versionSupported)
					{
						//Success message
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine(applicationProvided + " version " + versionProvided + " is supported by App2020");
						Console.WriteLine(applicationProvided + " launched.");
						Console.ResetColor();
					}
					else
					{
						//Version not supported message
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Version not supported by App 2020... ");
						Console.WriteLine("Launch of " + applicationProvided + " aborted.");
						Console.ResetColor();
					}
				}
				else
				{
					//Application not supported message
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Application not supported by App 2020... ");
					Console.WriteLine("Launch of " + applicationProvided + " aborted.");
					Console.ResetColor();
				}

				Console.WriteLine("\nLaunch another application?(y/n)");
				running = Console.ReadLine() == "y";		//Repeat until user wishes to stop running apps
			}
		}
	}
}
