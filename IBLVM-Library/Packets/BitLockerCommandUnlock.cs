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
    public class BitLockerUnlockCommand : BasePacket, IPayload<BitLockerUnlock>
    {
        public BitLockerUnlock Payload { get; private set; }

        private readonly CryptoMemoryStream cryptor;

        public BitLockerUnlockCommand(BitLockerUnlock bitlockerUnlock, CryptoMemoryStream cryptor) : base(PacketType.ServerBitLockerUnlockCommand)
        {
            Payload = bitlockerUnlock;
            this.cryptor = cryptor;
        }

        public override int GetPayloadSize() => base.GetPayloadSize() + Encoding.UTF8.GetByteCount(Payload.ToString());

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Payload.ToString());
            byte[] encryptedBytes = new byte[passwordBytes.Length];

            cryptor.Encrypt(passwordBytes, 0, passwordBytes.Length);
            cryptor.Read(encryptedBytes, 0, encryptedBytes.Length);

            stream.Write(encryptedBytes, 0, encryptedBytes.Length);
            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
            byte[] encryptedBytes = Utils.ReadFull(stream, payloadSize);
            byte[] data = new byte[encryptedBytes.Length];

            cryptor.Decrypt(encryptedBytes, 0, encryptedBytes.Length);
            cryptor.Read(data, 0, data.Length);

			Payload = BitLockerUnlock.FromString(Encoding.UTF8.GetString(data));
        }
    }
}
