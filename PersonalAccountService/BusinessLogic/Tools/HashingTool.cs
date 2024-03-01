using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tools
{
	public static class HashingTool
	{
		static public Dictionary<string, string> GenerateHash(string password)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();

			byte[] salt = GenerateSalt();
			byte[] passwordBytes = UTF8Encoding.UTF8.GetBytes(password);

			//byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
			//Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
			//Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);
			//using var hash = new SHA256CryptoServiceProvider();

			using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, 1000, HashAlgorithmName.SHA256);

			string hashPassword = Convert.ToBase64String(pbkdf2.GetBytes(32))/*ByteArrayToString(pbkdf2.GetBytes(32))*/;
			string saltStr = Convert.ToBase64String(salt)/*ByteArrayToString(salt)*/;

			dictionary.Add("salt", saltStr);
			dictionary.Add("hash", hashPassword);

			return dictionary;
		}

		static private string ByteArrayToString(byte[] byteArray)
		{
			var sOutput = new StringBuilder();

            foreach (var item in byteArray)
            {
				sOutput.Append(item.ToString("X2"));
            }

			return sOutput.ToString();
        }

		static private byte[] GenerateSalt()
		{
			const int SaltLength = 16;
			byte[] salt = new byte[SaltLength];

			var rngRand = new RNGCryptoServiceProvider();
			rngRand.GetBytes(salt);

			return salt;
		}
	}
}
