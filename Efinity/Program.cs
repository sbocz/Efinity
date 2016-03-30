using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppFinity
{
	/// <summary>
	/// Runs the demos for AppFinity. Can run either the Installation 
	/// demo or the Launch demo.
	/// </summary>
	/// <author>
	/// Sean Boczulak, 2016
	/// </author>
	class Program
	{
		static void Main(string[] args)
		{
			Database db = new Database();
			Console.WriteLine("AppFinity Launched.");

			bool running = true;

			//Repeat until user wishes to stop running apps
			while (running)
			{
				bool validDemo = false;
				int demoType = 0;
				while (!validDemo)
				{
					try
					{
						Console.WriteLine("Install(1) or Launch(2) application. View safe applications file(3).");
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
						InstallApplicationDemo(db);
						break;
					case 2:
						LaunchApplicationDemo(db);
						break;
					case 3:
						DisplaySafeHashFile();
						break;
				}
				Console.WriteLine("\nRun again?(y/n)");
				running = Console.ReadLine() == "y";        //Repeat until user wishes to stop running apps
			}
		}
		
		private static void DisplaySafeHashFile()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Demo for AppFinity launching an application.
		/// </summary>
		/// <param name="db">Database to use</param>
		private static void LaunchApplicationDemo(Database db)
		{
			//bool running = true;

			////Repeat until user wishes to stop running apps
			//while (running)
			//{
			//	Console.WriteLine("\nEnter an application name to run:");
			//	var applicationProvided = Console.ReadLine();
			//	bool versionInt = false;
			//	int versionProvided = 0;

			//	//Make sure version is an integer
			//	while (!versionInt)
			//	{
			//		try
			//		{
			//			Console.WriteLine("Enter the application version:");
			//			versionProvided = Convert.ToInt32(Console.ReadLine());
			//			versionInt = true;
			//		}
			//		catch (Exception)
			//		{
			//			versionInt = false;
			//		}
			//	}

			//	//Check for application being supported
			//	Console.WriteLine("\nChecking database for \"" + applicationProvided + "\"...");
			//	bool appSupported = false;
			//	foreach (DataRow row in db.Application.Rows)
			//	{
			//		if ((string)row["ApplicationName"] == applicationProvided)
			//		{
			//			appSupported = true;
			//			break;
			//		}
			//	}

			//	if (appSupported)
			//	{
			//		Console.WriteLine("Application supported by App2020...");
			//		Console.WriteLine("\nGetting \"" + applicationProvided + "\"s hash value...");

			//		//Check for application + version being supported
			//		Console.WriteLine("Checking database for \"" + applicationProvided + "\"s hash value...\n");
			//		bool versionSupported = false;
			//		foreach (DataRow row in db.Version.Rows)
			//		{
			//			if ((string)row["ApplicationName"] == applicationProvided && (int)row["VersionNumber"] == versionProvided && Hash.Confirm(applicationProvided + versionProvided + "Data", (string)row["HashKey"]))
			//			{
			//				versionSupported = true;
			//				break;
			//			}
			//		}
			//		if (versionSupported)
			//		{
			//			//Success message
			//			Console.ForegroundColor = ConsoleColor.Green;
			//			Console.WriteLine(applicationProvided + " version " + versionProvided + " is supported by App2020");
			//			Console.WriteLine(applicationProvided + " launched.");
			//			Console.ResetColor();
			//		}
			//		else
			//		{
			//			//Version not supported message
			//			Console.ForegroundColor = ConsoleColor.Red;
			//			Console.WriteLine("Version not supported by App 2020... ");
			//			Console.WriteLine("Launch of " + applicationProvided + " aborted.");
			//			Console.ResetColor();
			//		}
			//	}
			//	else
			//	{
			//		//Application not supported message
			//		Console.ForegroundColor = ConsoleColor.Red;
			//		Console.WriteLine("Application not supported by App 2020... ");
			//		Console.WriteLine("Launch of " + applicationProvided + " aborted.");
			//		Console.ResetColor();
			//	}

			//	Console.WriteLine("\nLaunch another application?(y/n)");
			//	running = Console.ReadLine() == "y";        //Repeat until user wishes to stop running apps
			//}
		}

		/// <summary>
		/// Demo for AppFinity verifying an application after installation.
		/// </summary>
		/// <param name="db">Database to use</param>
		private static string InstallApplicationDemo(Database db)
		{
			
			Console.WriteLine("\nEnter an application to install:");
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


			if (db.SupportsApplication(applicationProvided))
			{
				string appData = applicationProvided + versionProvided + "Data";
				Console.WriteLine("Application supported by App2020...");
				Console.WriteLine("\nComputing \"" + applicationProvided + "\" version " + versionProvided + "s hash value...");
				string hashValue = Hash.ComputeHash(appData);
				Console.WriteLine("Hash value: " + hashValue);

				//Check for application + version being supported
				Console.WriteLine("Checking database for \"" + applicationProvided + "\"s hash value...\n");
					
				if (db.VerifyApplication(applicationProvided, versionProvided, appData))
				{
					//Success message
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(applicationProvided + " version " + versionProvided + " is supported by App2020");
					Console.WriteLine(applicationProvided + " marked as safe.");
					Console.ResetColor();
					return hashValue;
				}
				else
				{
					//Version not supported message
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Version not supported by App 2020... ");
					Console.WriteLine("Update to a newer version of " + applicationProvided + " to be able to use it.");
					Console.ResetColor();
					return null;
				}
			}
			else
			{
				//Application not supported message
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Application not supported by App 2020... ");
				Console.WriteLine("Launch of " + applicationProvided + " aborted.");
				Console.ResetColor();
				return null;
			}
		}
	}
}
