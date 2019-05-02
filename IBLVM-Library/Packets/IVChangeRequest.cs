using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;

namespace IBLVM_Library.Packets
{
	public sealed class IVChangeRequest : BasePacket, IPayload<byte[]>
	{
        public byte[] Payload { get; private set; }

        public IVChangeRequest(byte[] initializeVector) : base(PacketType.IVChangeReqeust)
		{
            Payload = initializeVector;
		}

        public override int GetPayloadSize() => base.GetPayloadSize() + Payload.Length;

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            stream.Write(Payload, 0, Payload.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);

            Payload = Utils.ReadFull(stream, payloadSize);
        }
	}
}
