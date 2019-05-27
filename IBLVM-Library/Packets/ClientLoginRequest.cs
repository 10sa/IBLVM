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
	public sealed class ClientLoginRequest : BasePacket, IPayload<IAuthInfo>
	{
        public IAuthInfo Payload { get; private set; }

		private readonly CryptoMemoryStream cryptor;

		public ClientLoginRequest(string id, string password, ClientType type, CryptoMemoryStream cryptor) : base(PacketType.ClientLoginRequest)
		{
            Payload = new AuthInfo(new Account(id, password), type);
			this.cryptor = cryptor;
		}

		public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(Payload.ToString());

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] datas = Encoding.UTF8.GetBytes(Payload.ToString());
			cryptor.Encrypt(datas, 0, datas.Length);
			cryptor.WriteTo(buffer);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] buffer = new byte[payloadSize];

			Utils.ReadFull(stream, buffer, payloadSize);
			cryptor.Write(buffer, 0, buffer.Length);
			cryptor.Decrypt(buffer, 0, buffer.Length);
			string str = Encoding.UTF8.GetString(buffer);

			Payload = AuthInfo.FromString(Account.FromString(str), str);
		}
	}
}
