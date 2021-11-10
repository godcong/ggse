using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace guigubahuang
{
    public class DE
    {
        private const string key = "5:.A%KL;,.?<aH._=-/DF4s";

        private const string inner_key = "2a;ad.,&fSf^SX.,:12@D";

		public static void CreatJson(string jsonPath, string dePath)
		{
			if (jsonPath.Length != 0 && File.Exists(jsonPath))
			{
				string directoryName = Path.GetDirectoryName(jsonPath);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonPath);
				string text;
				try
				{
					text = NormalFileName(fileNameWithoutExtension);
				}
				catch
				{
					text = fileNameWithoutExtension;
				}
				byte[] bytes = Compress(Mult(File.ReadAllBytes(jsonPath), key, inner_key, isEncrypt: false), isCompress: false);
				File.WriteAllBytes(dePath, bytes);
			}
		}

		private static string NormalFileName(string name)
		{
			name = name.Replace('@', '/');
			byte[] data = Convert.FromBase64String(name);
			data = ProcessDES(data, key, isEncrypt: false);
			return Encoding.UTF8.GetString(data);
		}

		private static byte[] ProcessDES(byte[] data, string key, bool isEncrypt)
		{
			byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(array, 0, 8);
			ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(array, 8, 8);
			ICryptoTransform transform = ((!isEncrypt) ? dESCryptoServiceProvider.CreateDecryptor(arraySegment.ToArray(), arraySegment2.ToArray()) : dESCryptoServiceProvider.CreateEncryptor(arraySegment.ToArray(), arraySegment2.ToArray()));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();
			byte[] result = memoryStream.ToArray();
			memoryStream.Close();
			return result;
		}

		private static byte[] Mult(byte[] file, string key, string inner_key, bool isEncrypt)
		{
			byte[] array;
			if (isEncrypt)
			{
				array = file;
				byte[] bytes = Encoding.UTF8.GetBytes(key);
				byte[] bytes2 = Encoding.UTF8.GetBytes(inner_key);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (byte)(array[i] + bytes[i % bytes.Length]);
				}
				byte[] array2 = new byte[inner_key.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = (byte)(bytes2[j] ^ 3u);
				}
				array = array2.Concat(array).ToArray();
			}
			else
			{
				array = file.Skip(inner_key.Length).ToArray();
				byte[] bytes3 = Encoding.UTF8.GetBytes(key);
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (byte)(array[k] - bytes3[k % bytes3.Length]);
				}
			}
			return array;
		}

		private static byte[] Compress(byte[] file, bool isCompress)
		{
			if (!isCompress)
			{
				MemoryStream baseInputStream = new MemoryStream(file);
				MemoryStream memoryStream = new MemoryStream();
				GZipInputStream gZipInputStream = new GZipInputStream(baseInputStream);
				byte[] array = new byte[4096];
				while (true)
				{
					int num = gZipInputStream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					memoryStream.Write(array, 0, num);
				}
				byte[] result = memoryStream.ToArray();
				gZipInputStream.Close();
				memoryStream.Close();
				return result;
			}
			MemoryStream memoryStream2 = new MemoryStream();
			new GZipOutputStream(memoryStream2).Write(file, 0, file.Length);
			byte[] result2 = memoryStream2.ToArray();
			memoryStream2.Close();
			return result2;
		}
	}
}
