using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;

namespace IBLVM_Library.Models
{
	public sealed class IVChangeRequest : BasePacket, ICryptoExchanger
	{
		public byte[] Data { get; private set; }

		public IVChangeRequest(byte[] initializeVector) : base(PacketType.IVChangeReqeust)
		{
            Data = initializeVector;
		}

        public override int GetPayloadSize() => base.GetPayloadSize() + Data.Length;

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            stream.Write(Data, 0, Data.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);

            Data = Utils.ReadFull(stream, payloadSize);
        }
	}
}
