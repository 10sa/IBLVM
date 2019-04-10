using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	sealed class ServerKeyResponse : BasePacket
	{
		private readonly byte[] cryptoKey;

		public ServerKeyResponse(byte[] cryptoKey) : base(Enums.PacketType.ServerKeySend)
		{
			this.cryptoKey = cryptoKey;
		}

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			WriteToStream(buffer, cryptoKey);

			return buffer;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + cryptoKey.Length;
	}
}
