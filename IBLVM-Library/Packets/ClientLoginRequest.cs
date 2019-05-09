using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecureStream;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

namespace IBLVM_Library.Packets
{
	public sealed class ClientLoginRequest : BasePacket, IPayload<IAccount>
	{
        public IAccount Payload { get; private set; }

		private readonly CryptoMemoryStream cryptor;

		public ClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) : base(PacketType.ClientLoginRequest)
		{
            Payload = new Account(id, password);
			this.cryptor = cryptor;
		}

		public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(Payload.Id) + Encoding.UTF8.GetByteCount(Payload.Password) + 1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			byte[] id = Encoding.UTF8.GetBytes(Payload.Id);
			byte[] password = Encoding.UTF8.GetBytes(Payload.Password);

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
			string id = Encoding.UTF8.GetString(datas, 0, teminateOffset - 1);
			string password = Encoding.UTF8.GetString(datas, teminateOffset, datas.Length - teminateOffset);

            Payload = new Account(id, password);
		}
	}
}
