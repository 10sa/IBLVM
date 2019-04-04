using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	public class ClientKeyResponse : BasePacket
	{
		private readonly byte[] cryptoKey;

		public ClientKeyResponse(byte[] cryptoKey) : base(PacketType.ClientKeySend)
		{
			this.cryptoKey = cryptoKey;
		}

		public ClientKeyResponse(byte[] data, ref int offset) : base(data, ref offset)
		{
			cryptoKey = new byte[data.Length - offset];
			Array.Copy(cryptoKey, 0, data, offset, data.Length - offset);
		}

		public override void GetPayload(Stream buffer)
		{
			base.GetPayload(buffer);
			WriteToStream(buffer, cryptoKey);
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + cryptoKey.Length;
	}
}
