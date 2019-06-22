using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
	public class ServerBitLockerUnlock
	{
		public DriveInformation Drive { get; private set; }

		public string Password { get; private set; }

		private readonly CryptoProvider cryptor;

		public ServerBitLockerUnlock(DriveInformation drive, string password, CryptoProvider cryptor)
		{
			Drive = drive;
			Password = password;
			this.cryptor = cryptor;
		}

		public override string ToString()
		{
			byte[] encryptedData = new byte[Password.Length];
			byte[] data = Encoding.UTF8.GetBytes(Password);
			cryptor.CryptoStream.Encrypt(data, 0, data.Length);
			cryptor.CryptoStream.Read(encryptedData, 0, encryptedData.Length);

			string password = Convert.ToBase64String(encryptedData);
			return $"{Drive},{password}";
		}

		public static ServerBitLockerUnlock FromString(string str, CryptoProvider decryptor)
		{
			string[] datas = str.Split(',');
			byte[] password = Convert.FromBase64String(datas.Last());
			byte[] decryptPassword = new byte[password.Length];
			decryptor.CryptoStream.Write(password, 0, password.Length);
			decryptor.CryptoStream.Decrypt(decryptPassword, 0, decryptPassword.Length);

			return new ServerBitLockerUnlock(DriveInformation.FromString(str), Encoding.UTF8.GetString(decryptPassword, 0, decryptPassword.Length), decryptor);
		}
	}
}
