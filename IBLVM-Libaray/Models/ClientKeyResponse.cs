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
	public sealed class ClientKeyResponse : BasePacket
	{
		public byte[] Key { get; set; }

		public ClientKeyResponse(byte[] key) : base(PacketType.ClientKeyResponse)
		{
			Key = key;
		}

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();
			WriteToStream(stream, Key);

			return stream;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + Key.Length;

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			this.Key = StreamUtil.ReadFull(stream, payloadSize);
		}
	}
}
