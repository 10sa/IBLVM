using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecureStream;

using IBLVM_Libaray.Enums;

namespace IBLVM_Libaray.Models
{
	public sealed class ClientLoginRequest : BasePacket
	{
		public string Id { get; private set; }

		public string Password { get; private set; }

		private readonly CryptoMemoryStream cryptor;

		public ClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) : base(PacketType.ClientLoginRequest)
		{
			Id = id;
			Password = password;
			this.cryptor = cryptor;
		}

		public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(Id) + Encoding.UTF8.GetByteCount(Password) + 1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			byte[] id = Encoding.UTF8.GetBytes(Id);
			byte[] password = Encoding.UTF8.GetBytes(Password);

			byte[] datas = new byte[GetPayloadSize()];
			id.CopyTo(datas, 0);
			password.CopyTo(datas, id.Length + 1);

			cryptor.Encrypt(datas, 0, datas.Length);

			byte[] encryptBytes = new byte[id.Length + password.Length + 1];
			cryptor.Read(encryptBytes, 0, encryptBytes.Length);
			buffer.Write(encryptBytes, 0, encryptBytes.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] encryptBytes = new byte[payloadSize];
			byte[] datas = new byte[payloadSize];

			Utils.ReadFull(stream, encryptBytes, payloadSize);
			cryptor.Write(encryptBytes, 0, encryptBytes.Length);
			cryptor.Decrypt(datas, 0, datas.Length);

			int teminateOffset = Array.IndexOf(datas, (byte)0x0) + 1;
			Id = Encoding.UTF8.GetString(datas, 0, teminateOffset - 1);
			Password = Encoding.UTF8.GetString(datas, teminateOffset, datas.Length - teminateOffset);
		}
	}
}
