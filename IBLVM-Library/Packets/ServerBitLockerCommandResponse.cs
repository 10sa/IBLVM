using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Packets
{
	class ServerBitLockerCommandResponse : BasePacket, IPayload<bool>
	{
		public bool Payload { get; private set; }

		public ServerBitLockerCommandResponse(bool payload) : base(PacketType.ServerBitLockerCommandResponse)
		{
			Payload = payload;
		}

		public override int GetPayloadSize() => sizeof(bool);

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] data = BitConverter.GetBytes(Payload);
			buffer.Write(data, 0, data.Length);
			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			if (payloadSize != sizeof(bool))
				throw new InvalidDataException("Invalid payload size!");

			Payload = BitConverter.ToBoolean(Utils.ReadFull(stream, payloadSize), 0);
		}
	}
}
