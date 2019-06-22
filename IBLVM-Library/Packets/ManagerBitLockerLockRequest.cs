using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Models;

using SecureStream;
using System.IO;

namespace IBLVM_Library.Packets
{
    public class ManagerBitLockerLockRequest : BasePacket, IPayload<ClientDrive>
    {
        public ClientDrive Payload { get; private set; }

        public ManagerBitLockerLockRequest(ClientDrive drive) : base(PacketType.ServerBitLockerLockCommand)
        {
			Payload = drive;
        }

        public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(Payload.ToString());

        public override Stream GetPayloadStream()
        {
            Stream buffer = base.GetPayloadStream();
            byte[] bytes = Encoding.UTF8.GetBytes(Payload.ToString());
            buffer.Write(bytes, 0, bytes.Length);

            return buffer;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
			Payload = ClientDrive.FromString(Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)));
		}
    }
}
