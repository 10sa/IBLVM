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

		public ClientKeyResponse(byte[] cryptoKey) : base(PacketType.ClientKeyResponse)
		{
			this.cryptoKey = cryptoKey;
		}

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();
			WriteToStream(stream, cryptoKey);

			return stream;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + cryptoKey.Length;
	}
}
