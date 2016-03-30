using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppFinity
{
	/// <summary>
	/// Runs the demos for AppFinity. Can run either the Installation 
	/// demo or the Launch demo. Demo is run as if the device has nothing
	/// installed on it.
	/// </summary>
	/// <author>
	/// Sean Boczulak, 2016
	/// </author>
	class Program
	{
		//Use any file path you want
		private const string HASH_FILE_PATH = @"C:\Users\Sean\Desktop\HashFile.txt";

		static void Main(string[] args)
		{
			Database db = new Database();
			File.Create(HASH_FILE_PATH).Close();
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
						WriteToHashFile(InstallApplicationDemo(db), HASH_FILE_PATH);
						break;
					case 2:
						LaunchApplicationDemo(db);
						break;
					case 3:
						DisplayHashFile(HASH_FILE_PATH);
						break;
				}
				Console.WriteLine("\nRun again?(y/n)");
				running = Console.ReadLine() == "y";        //Repeat until user wishes to stop running apps
			}
		}

		/// <summary>
		/// Read the hash file for validated apps
		/// to be run using AppFinity.
		/// </summary>
		/// <param name="filePath">File to get the hashValues from.</param>
		private static string[] ReadHashFile(string filePath)
		{

			List<string> hashValues = new List<string>();
			try
			{
				StreamReader reader = new StreamReader(filePath);
				string line = reader.ReadLine();

				while (!string.IsNullOrEmpty(line))
				{
					hashValues.Add(line);
					line = reader.ReadLine();
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
			return hashValues.ToArray();
		}

		/// <summary>
		/// Writes the hash value to the hash file for safe 
		/// applications
		/// </summary>
		/// <param name="toWrite">
		/// Hash Value to write. If null nothing is written.
		/// </param>
		/// <param name="filePath"></param>
		private static void WriteToHashFile(string toWrite, string filePath)
		{
			//Save file and backup file
			StreamWriter writer = new StreamWriter(filePath, true);
			if(toWrite != null)
				writer.WriteLine(toWrite);
			writer.Close();
		}

		/// <summary>
		/// Display contents of hash file
		/// </summary>
		/// <param name="filePath"></param>
		private static void DisplayHashFile(string filePath)
		{
			Console.WriteLine();
			foreach (string s in ReadHashFile(filePath))
			{
				Console.WriteLine(s);
			}
		}

		/// <summary>
		/// Demo for AppFinity launching an application.
		/// </summary>
		/// <param name="db">Database to use</param>
		private static void LaunchApplicationDemo(Database db)
		{
			Console.WriteLine("\nEnter an application name to run:");
			var applicationProvided = Console.ReadLine();
			bool versionInt = false;
			int versionProvided = 0;
			bool applicationSafe = false;
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

			string appData = applicationProvided + versionProvided + "Data";
			Console.WriteLine("\nComputing \"" + applicationProvided + "\" version " + versionProvided + "s hash value...");

			//Check for application being supported
			Console.WriteLine("Checking if \"" + applicationProvided + "\" is marked safe...");
			foreach (string s in ReadHashFile(HASH_FILE_PATH))
			{
				if (Hash.Confirm(appData, s))
				{
					applicationSafe = true;
					break;
				}
			}

			if (applicationSafe)
			{
				//Application safe message
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(applicationProvided + " version " + versionProvided + " is marked safe...");
				Console.WriteLine(applicationProvided + " launched.");
				Console.ResetColor();
			}
			else
			{
				//Application not safe message
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(applicationProvided + " version " + versionProvided + " is not marked safe...");
				Console.WriteLine("File is either corrupt or was incorrectly installed. Please reinstall " 
					+ applicationProvided + ". Launch aborted.");
				Console.ResetColor();
			}
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
