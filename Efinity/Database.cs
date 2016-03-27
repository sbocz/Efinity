using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Efinity
{
	/// <summary>
	/// Fake database for demo purposes
	/// </summary>
	class Database
	{
		public readonly DataTable Version;
		public readonly DataTable Application;
		
		public Database()
		{
			//Create Application Table
			Application = new DataTable();
			Application.Columns.Add("ApplicationName", typeof(string));
			Application.Columns.Add("HashKey", typeof(string));

			//Approved applications
			Application.Rows.Add("Skype", Hash.ComputeHash("Skype", null));
			Application.Rows.Add("App2020 Store", Hash.ComputeHash("App2020 Store", null));
			Application.Rows.Add("FireFox", Hash.ComputeHash("FireFox", null));
			Application.Rows.Add("Flappy Bird", Hash.ComputeHash("Flappy Bird", null));


			//Create Version Table
			Version = new DataTable();
			Version.Columns.Add("ApplicationName", typeof(string));
			Version.Columns.Add("VersionNumber", typeof(int));
			Version.Columns.Add("HashKey", typeof(string));

			//Supported Versions
			Version.Rows.Add("Skype", 4, Hash.ComputeHash("Skype4", null));
			Version.Rows.Add("Skype", 5, Hash.ComputeHash("Skype5", null));
			Version.Rows.Add("Skype", 6, Hash.ComputeHash("Skype6", null));
			Version.Rows.Add("Skype", 7, Hash.ComputeHash("Skype7", null));

			Version.Rows.Add("App2020 Store", 12, Hash.ComputeHash("App2020 Store12", null));
			Version.Rows.Add("App2020 Store", 13, Hash.ComputeHash("App2020 Store13", null));
			Version.Rows.Add("App2020 Store", 14, Hash.ComputeHash("App2020 Store14", null));

			Version.Rows.Add("FireFox", 39, Hash.ComputeHash("FireFox39", null));
			Version.Rows.Add("FireFox", 40, Hash.ComputeHash("FireFox40", null));
			Version.Rows.Add("FireFox", 41, Hash.ComputeHash("FireFox41", null));
			Version.Rows.Add("FireFox", 42, Hash.ComputeHash("FireFox42", null));
			Version.Rows.Add("FireFox", 43, Hash.ComputeHash("FireFox43", null));
			Version.Rows.Add("FireFox", 44, Hash.ComputeHash("FireFox44", null));
			Version.Rows.Add("FireFox", 45, Hash.ComputeHash("FireFox45", null));

			Version.Rows.Add("Flappy Bird", 1, Hash.ComputeHash("Flappy Bird1", null));
			Version.Rows.Add("Flappy Bird", 2, Hash.ComputeHash("Flappy Bird2", null));
		}
	}
}
