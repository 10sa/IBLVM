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

		private CryptoMemoryStream cryptor;

        public IVChangeRequest(byte[] initializeVector, CryptoMemoryStream cryptor) : base(PacketType.IVChangeReqeust)
		{
            Payload = initializeVector;
			this.cryptor = cryptor;
		}

        public override int GetPayloadSize() => base.GetPayloadSize() + Payload.Length;

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
			cryptor.Encrypt(Payload, 0, Payload.Length);

			byte[] encryptedIV = Utils.ReadFull(cryptor, Payload.Length);
			stream.Write(encryptedIV, 0, encryptedIV.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);

			Payload = new byte[payloadSize];
            byte[] encryptedIV = Utils.ReadFull(stream, payloadSize);
			cryptor.Write(encryptedIV, 0, encryptedIV.Length);
			cryptor.Decrypt(Payload, 0, Payload.Length);
        }
	}
}
