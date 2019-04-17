using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
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
			this.Data = StreamUtil.ReadFull(stream, payloadSize);
		}
	}
}
