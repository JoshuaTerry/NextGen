using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DDI.Shared
{
	public class Encryption
	{ 
        public static string Encrypt(string plainText)
        {
            string passPhrase = "DD1SecretPhr@s3";
            string saltValue = "5@ltV@LU3";
            string hashAlgorithm = "SHA1";
            int iterations = 1024;
            string initVector = "@1B2c3D4e5F6g7H8";
            int keySize = 256;

            return Encryption.Encrypt(plainText, passPhrase, saltValue, hashAlgorithm, iterations, initVector, keySize);
        }
		public static string Encrypt(string   plainText,
			string   passPhrase,
			string   saltValue,
			string   hashAlgorithm,
			int      passwordIterations,
			string   initVector,
			int      keySize)
		{ 
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);         
			byte[] plainTextBytes  = Encoding.UTF8.GetBytes(plainText);
        			
			PasswordDeriveBytes password = new PasswordDeriveBytes( passPhrase,  saltValueBytes, hashAlgorithm, passwordIterations); 

			byte[] keyBytes = password.GetBytes(keySize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };       
			
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            string cipherText = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherTextBytes = memoryStream.ToArray();
                    cipherText = Convert.ToBase64String(cipherTextBytes);
                }
            }			
        
			return cipherText;
		}
           
        public static string Decrypt(string encyrptedText)
        {
            string passPhrase = "DD1SecretPhr@s3";
            string saltValue = "5@ltV@LU3";
            string hashAlgorithm = "SHA1";
            int iterations = 1024;
            string initVector = "@1B2c3D4e5F6g7H8";
            int keySize = 256;

            return Encryption.Decrypt(encyrptedText, passPhrase, saltValue, hashAlgorithm, iterations, initVector, keySize);
        }
        public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };
        
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,	initVectorBytes);
            string plainText = string.Empty;

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }		
        
			return plainText;
		}
	}
}
