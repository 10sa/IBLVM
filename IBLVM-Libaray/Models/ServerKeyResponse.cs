using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	public sealed class ServerKeyResponse : BasePacket
	{
		public byte[] Key { get; private set; }

		public ServerKeyResponse(byte[] cryptoKey) : base(Enums.PacketType.ServerKeyResponse)
		{
			Key = cryptoKey;
		}

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			WriteToStream(buffer, Key);

			return buffer;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + Key.Length;

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			Key = StreamUtil.ReadFull(stream, payloadSize);
		}
	}
}
