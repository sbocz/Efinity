using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AppFinity
{
	/// <summary>
	/// Makeshift database for demo purposes only.
	/// </summary>
	/// <author>
	/// Sean Boczulak, 2016
	/// </author>
	class Database
	{
		private readonly DataTable Version;
		private readonly DataTable Application;
		
		public Database()
		{
			//Create Application Table
			Application = new DataTable();
			Application.Columns.Add("ApplicationName", typeof(string));

			//Approved applications
			Application.Rows.Add("Skype");
			Application.Rows.Add("App2020 Store");
			Application.Rows.Add("FireFox");
			Application.Rows.Add("Flappy Bird");


			//Create Version Table
			Version = new DataTable();
			Version.Columns.Add("ApplicationName", typeof(string));
			Version.Columns.Add("VersionNumber", typeof(int));
			Version.Columns.Add("HashKey", typeof(string));

			//Supported Versions
			Version.Rows.Add("Skype", 4, Hash.ComputeHash("Skype4Data", null));
			Version.Rows.Add("Skype", 5, Hash.ComputeHash("Skype5Data", null));
			Version.Rows.Add("Skype", 6, Hash.ComputeHash("Skype6Data", null));
			Version.Rows.Add("Skype", 7, Hash.ComputeHash("Skype7Data", null));

			Version.Rows.Add("App2020 Store", 12, Hash.ComputeHash("App2020 Store12Data", null));
			Version.Rows.Add("App2020 Store", 13, Hash.ComputeHash("App2020 Store13Data", null));
			Version.Rows.Add("App2020 Store", 14, Hash.ComputeHash("App2020 Store14Data", null));

			Version.Rows.Add("FireFox", 39, Hash.ComputeHash("FireFox39Data", null));
			Version.Rows.Add("FireFox", 40, Hash.ComputeHash("FireFox40Data", null));
			Version.Rows.Add("FireFox", 41, Hash.ComputeHash("FireFox41Data", null));
			Version.Rows.Add("FireFox", 42, Hash.ComputeHash("FireFox42Data", null));
			Version.Rows.Add("FireFox", 43, Hash.ComputeHash("FireFox43Data", null));
			Version.Rows.Add("FireFox", 44, Hash.ComputeHash("FireFox44Data", null));
			Version.Rows.Add("FireFox", 45, Hash.ComputeHash("FireFox45Data", null));

			Version.Rows.Add("Flappy Bird", 1, Hash.ComputeHash("Flappy Bird1Data", null));
			Version.Rows.Add("Flappy Bird", 2, Hash.ComputeHash("Flappy Bird2Data", null));
		}

		public bool VerifyApplication(string appName, int appVersion, string appData)
		{
			foreach (DataRow row in Version.Rows)
			{
				if ((string)row["ApplicationName"] == appName && (int)row["VersionNumber"] == appVersion && Hash.Confirm(appData, (string)row["HashKey"]))
				{
					return true;
				}
			}
			return false;
		}

		public bool SupportsApplication(string appName)
		{
			foreach (DataRow row in Application.Rows)
			{
				if ((string)row["ApplicationName"] == appName)
				{
					return true;
				}
			}
			return false;
		}
	}
}
