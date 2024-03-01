using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections;

namespace BusinessLogic.Tools
{
	public static class PasswordsComparer
	{
		public static bool ComparePasswords(string hashPassword, string inputPassword, string salt)
		{
			byte[] saltBytes = Convert.FromBase64String(salt);
			byte[] inputPasswordBytes = UTF8Encoding.UTF8.GetBytes(inputPassword);

			//byte[] saltedInputPasswordBytes = new byte[inputPasswordBytes.Length + saltBytes.Length];
			//Buffer.BlockCopy(inputPasswordBytes, 0, saltedInputPasswordBytes, 0, inputPasswordBytes.Length);
			//Buffer.BlockCopy(saltBytes, 0, saltedInputPasswordBytes, inputPasswordBytes.Length, saltBytes.Length);

			using var pbkdf2 = new Rfc2898DeriveBytes(inputPasswordBytes, saltBytes, 1000, HashAlgorithmName.SHA256);

			return CompareStrings(hashPassword, Convert.ToBase64String(pbkdf2.GetBytes(32)));
		}

		private static bool CompareByteArrays(byte[] array1, byte[] array2)
		{
			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
					return false;
			}

			return true;
		}

		private static bool CompareStrings(string str1, string str2)
		{
			return str1.Equals(str2);
		}
	}
}
