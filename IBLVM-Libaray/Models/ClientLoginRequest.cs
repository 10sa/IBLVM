using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecureStream;

using IBLVM_Libaray.Enums;
using Iblvm;

namespace IBLVM_Libaray.Models
{
	class ClientLoginRequest : BasePacket
	{
		public string Id { get; private set; }

		public string Password { get; private set; }

		private CryptoMemoryStream cryptor;

		public ClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) : base(PacketType.ClientLoginRequest)
		{
			Id = id;
			Password = password;
			this.cryptor = cryptor;
		}

		public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(Id) + Encoding.UTF8.GetByteCount(Password);

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			byte[] id = Encoding.UTF8.GetBytes(Id);
			byte[] password = Encoding.UTF8.GetBytes(Password);

			cryptor.Encrypt(id, 0, id.Length);
			cryptor.Encrypt(password, 0, password.Length);

			byte[] encryptBytes = new byte[id.Length + password.Length + 2];
			cryptor.Read(encryptBytes, 0, encryptBytes.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] encryptBytes = new byte[payloadSize];
			byte[] datas = new byte[payloadSize];

			for (int i = 0; i < payloadSize;)
				i += stream.Read(encryptBytes, i, payloadSize - i);

			cryptor.Write(encryptBytes, 0, encryptBytes.Length);
			cryptor.Decrypt(datas, 0, datas.Length);

			int teminateOffset = Array.IndexOf(datas, 0x0);
			Id = Encoding.UTF8.GetString(datas, 0, teminateOffset);
			Password = Encoding.UTF8.GetString(datas, teminateOffset, datas.Length - teminateOffset);
		}
	}
}
