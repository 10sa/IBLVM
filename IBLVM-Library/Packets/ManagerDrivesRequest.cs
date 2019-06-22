using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

namespace IBLVM_Library.Packets
{
	class ManagerDrivesRequest : BasePacket, IPayload<IDevice>
	{
		public IDevice Payload { get; private set; }

		public ManagerDrivesRequest(IDevice device) : base(PacketType.ManagerDrivesRequest)
		{
			Payload = device;
		}

		public override int GetPayloadSize() => -1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] data = Encoding.UTF8.GetBytes(Payload.ToString());
			buffer.Write(data, 0, data.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			Payload = Device.FromString(Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)));
		}
	}
}
