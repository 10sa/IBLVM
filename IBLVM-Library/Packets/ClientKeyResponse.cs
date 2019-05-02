using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Packets
{
	public sealed class ClientKeyResponse : BasePacket, IPayload<byte[]>
	{
        public byte[] Payload { get; private set; }

        public ClientKeyResponse(byte[] key) : base(PacketType.ClientKeyResponse)
		{
			Payload = key;
		}

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();
			WriteToStream(stream, Payload);

			return stream;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + Payload.Length;

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			this.Payload = Utils.ReadFull(stream, payloadSize);
		}
	}
}
