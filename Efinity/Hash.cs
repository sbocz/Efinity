using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Efinity
{
	class Hash
	{
		/// <summary>
		/// Gets the resulting hash string from the text entered and salt bytes supplied.
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="salt">If null, random salt generated</param>
		/// <returns></returns>
		public static string ComputeHash(string plainText, byte[] salt = null)
		{
			int minSaltLength = 4, maxSaltLength = 8;

			byte[] saltBytes = null;
			if (salt != null)
			{
				saltBytes = salt;
			}
			else
			{
				Random r = new Random();
				int saltLength = r.Next(minSaltLength, maxSaltLength);
				saltBytes = new byte[saltLength];
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
				rng.GetNonZeroBytes(saltBytes);
				rng.Dispose();
			}

			byte[] plainData = Encoding.UTF8.GetBytes(plainText);
			byte[] plainDataWithSalt = new byte[plainData.Length + saltBytes.Length];

			for (int i = 0; i < plainData.Length; i++)
				plainDataWithSalt[i] = plainData[i];

			for (int i = 0; i < saltBytes.Length; i++)
				plainDataWithSalt[plainData.Length + i] = saltBytes[i];

			byte[] hashValue = null;

			SHA256Managed sha = new SHA256Managed();
			hashValue = sha.ComputeHash(plainDataWithSalt);
			sha.Dispose();
			

			byte[] result = new byte[hashValue.Length + saltBytes.Length];
			for (int i = 0; i < hashValue.Length; i++)
				result[i] = hashValue[i];

			for (int i = 0; i < saltBytes.Length; i++)
				result[hashValue.Length + i] = saltBytes[i];

			return Convert.ToBase64String(result);
		}

		public static bool Confirm(string plainText, string hashValue)
		{
			byte[] hashBytes = Convert.FromBase64String(hashValue);
			int hashSize = 32;	//bytes in SHA 256

			byte[] saltBytes = new byte[hashBytes.Length - hashSize];

			for (int i = 0; i < saltBytes.Length; i++)
				saltBytes[i] = hashBytes[hashSize + i];

			string newHash = ComputeHash(plainText, saltBytes);

			return (hashValue == newHash);
		}
	}
}
