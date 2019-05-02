using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Packets
{
	public sealed class ServerKeyResponse : BasePacket, IPayload<byte[]>
	{
        public byte[] Payload { get; private set; }

        public ServerKeyResponse(byte[] cryptoKey) : base(Enums.PacketType.ServerKeyResponse)
		{
			Payload = cryptoKey;
		}

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			WriteToStream(buffer, Payload);

			return buffer;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + Payload.Length;

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			Payload = Utils.ReadFull(stream, payloadSize);
		}
	}
}
