using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppFinity
{
	/// <summary>
	/// Used to confirm and compute hash signatures for files
	/// using the SHA-2 hashing algorithm with salt. 
	/// </summary>
	/// <author>
	/// Sean Boczulak, 2016
	/// </author>
	class Hash
	{
		/// <summary>
		/// Gets the resulting hash string from the text entered and 
		/// salt bytes supplied. Hashstring returned includes the 
		/// saltbytes appended at the end.
		/// </summary>
		/// <param name="textData"> Data to be hashed. </param>
		/// <param name="salt"> 
		/// Bytes for salting data. If null, random salt generated.
		/// </param>
		/// <returns></returns>
		public static string ComputeHash(string textData, byte[] salt = null)
		{
			
			//Get salt if it is not provided
			byte[] saltBytes = null;
			saltBytes = salt ?? GetRandomSalt();
			
			byte[] plainData = Encoding.UTF8.GetBytes(textData);
			
			//byte array for data + salt
			byte[] plainDataWithSalt = new byte[plainData.Length + saltBytes.Length];

			for (int i = 0; i < plainData.Length; i++)
				plainDataWithSalt[i] = plainData[i];

			for (int i = 0; i < saltBytes.Length; i++)
				plainDataWithSalt[plainData.Length + i] = saltBytes[i];

			//byte array for hashed data + salt
			byte[] hashValue = null;
			SHA256Managed sha = new SHA256Managed();
			hashValue = sha.ComputeHash(plainDataWithSalt);
			sha.Dispose();
			
			//Hash value + salt bytes
			byte[] result = new byte[hashValue.Length + saltBytes.Length];
			for (int i = 0; i < hashValue.Length; i++)
				result[i] = hashValue[i];

			for (int i = 0; i < saltBytes.Length; i++)
				result[hashValue.Length + i] = saltBytes[i];

			//Get string to return based on resulting bytes
			return Convert.ToBase64String(result);
		}

		private static byte[] GetRandomSalt()
		{
			Random r = new Random();
			byte[] saltBytes = new byte[4];	//Four bytes of salt
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetNonZeroBytes(saltBytes);
			//Dispose of object for security reasons
			rng.Dispose();	
			return saltBytes;
		}

		public static bool Confirm(string textData, string hashValue)
		{
			byte[] hashBytes = Convert.FromBase64String(hashValue);
			int hashSize = 32;  //bytes in SHA 256

			//Gets the saltbytes that would've been appended using our
			//hash function
			byte[] saltBytes = new byte[hashBytes.Length - hashSize];

			for (int i = 0; i < saltBytes.Length; i++)
				saltBytes[i] = hashBytes[hashSize + i];

			//Get hash from data to confirm it matches
			string newHash = ComputeHash(textData, saltBytes);
			if (hashValue == newHash)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(newHash);
				Console.ResetColor();
			}
			else
			{
				Console.WriteLine(newHash);
			}
			return (hashValue == newHash);
		}
	}
}
