using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;
using System.IO;

namespace IBLVM_Library.Models
{
    class IVChangeResponse : BasePacket, IActionResult
    {
        public bool Success { get; private set; }

        public IVChangeResponse(bool isSuccess) : base(PacketType.IVChangeResponse)
        {
            Success = isSuccess;
        }

        public override int GetPayloadSize() => base.GetPayloadSize() + sizeof(bool);

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            byte[] payload = BitConverter.GetBytes(Success);
            stream.Write(payload, 0, payload.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
            Success = BitConverter.ToBoolean(Utils.ReadFull(stream, payloadSize), 0);
        }
    }
}
