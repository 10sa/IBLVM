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
    public abstract class BaseActionResultPacket : BasePacket, IPayload<bool>
    {
        public bool Payload { get; private set; }

        public BaseActionResultPacket(bool isSuccess, PacketType type) : base(type) => Payload = isSuccess;

        public sealed override int GetPayloadSize() => base.GetPayloadSize() + sizeof(bool);

        public sealed override Stream GetPayloadStream()
        {
            Stream buffer = base.GetPayloadStream();
            byte[] data = BitConverter.GetBytes(Payload);

            buffer.Write(data, 0, data.Length);
            return buffer;
        }

        public sealed override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
            Payload = BitConverter.ToBoolean(Utils.ReadFull(stream, payloadSize), 0);
        }
    }
}
