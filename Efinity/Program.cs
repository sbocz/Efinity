using System;
using System.Collections.Generic;
using System.Data;
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

			string hashValue = Hash.ComputeHash("hello", null);
			bool good = Hash.Confirm("hello", hashValue);

			Console.WriteLine(good);
			Console.Read();
		}
	}
}
