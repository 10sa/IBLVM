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
    public class BitLockerUnlockCommand : BasePacket
    {
        public string Password { get; set; }

        private CryptoMemoryStream cryptor;

        public BitLockerUnlockCommand(string password, CryptoMemoryStream cryptor) : base(PacketType.ServerBitLockerUnlockCommand)
        {
            Password = password;
            this.cryptor = cryptor;
        }

        public override int GetPayloadSize() => base.GetPayloadSize() + Encoding.UTF8.GetByteCount(Password);

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);
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
            byte[] passwordBytes = new byte[encryptedBytes.Length];

            cryptor.Decrypt(encryptedBytes, 0, encryptedBytes.Length);
            cryptor.Read(passwordBytes, 0, passwordBytes.Length);

            Password = Encoding.UTF8.GetString(passwordBytes);
        }
    }
}
