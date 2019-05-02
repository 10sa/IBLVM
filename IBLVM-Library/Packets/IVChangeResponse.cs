using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;
using System.IO;

namespace IBLVM_Library.Packets
{
    class IVChangeResponse : BasePacket, IPayload<bool>
    {
        public bool Payload { get; private set; }

        public IVChangeResponse(bool isSuccess) : base(PacketType.IVChangeResponse)
        {
            Payload = isSuccess;
        }

        public override int GetPayloadSize() => base.GetPayloadSize() + sizeof(bool);

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            byte[] payload = BitConverter.GetBytes(Payload);
            stream.Write(payload, 0, payload.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
            Payload = BitConverter.ToBoolean(Utils.ReadFull(stream, payloadSize), 0);
        }
    }
}
