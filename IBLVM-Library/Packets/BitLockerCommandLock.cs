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
    public class BitLockerLockCommand : BasePacket, IPayload<BitLockerVolume>
    {
        public BitLockerVolume Payload { get; private set; }

        public BitLockerLockCommand(BitLockerVolume volume) : base(PacketType.ServerBitLockerLockCommand)
        {
            Payload = volume;
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
            string[] datas = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)).Split(';');
            Payload = new BitLockerVolume(datas[0], datas[1]);
        }
    }
}
