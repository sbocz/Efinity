using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Efinity
{
	/// <summary>
	/// Fake database for demo purposes
	/// </summary>
	class Database
	{
		public DataTable Version;
		public DataTable Application;
		
		public Database()
		{
			MakeApplicationTable();
			MakeVersionTable();
		}

		private void MakeApplicationTable()
		{
			Application = new DataTable();
			Application.Columns.Add("ApplicationName");
			Application.Columns.Add("HashKey");
		}

		private void MakeVersionTable()
		{
			Version = new DataTable();
			Version.Columns.Add("VersionNumber");
			Version.Columns.Add("ApplicationName");
			Version.Columns.Add("HashKey");
		}
	}
}
