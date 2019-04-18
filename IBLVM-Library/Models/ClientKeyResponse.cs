using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
{
	public sealed class ClientKeyResponse : BasePacket, ICryptoExchanger
	{
		public byte[] Data { get; private set; }

		public ClientKeyResponse(byte[] key) : base(PacketType.ClientKeyResponse)
		{
			Data = key;
		}

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();
			WriteToStream(stream, Data);

			return stream;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + Data.Length;

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			this.Data = Utils.ReadFull(stream, payloadSize);
		}
	}
}
