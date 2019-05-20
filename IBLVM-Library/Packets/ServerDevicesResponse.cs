using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Library.Models;
using IBLVM_Library.Packets;
using System.IO;

namespace IBLVM_Library.Packets
{
    public class ServerDevicesResponse : BasePacket, IPayload<IDevice[]>
    {
		public IDevice[] Payload { get; private set; }

		public ServerDevicesResponse(IDevice[] payload) : base(PacketType.ServerDevicesResponse)
		{
			Payload = payload;
		}

		public override int GetPayloadSize() => -1;

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();

			foreach(var payload in Payload)
			{
				byte[] datas = Encoding.UTF8.GetBytes(payload.ToString() + ';');
				stream.Write(datas, 0, datas.Length);
			}

			return stream;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			string[] devices = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)).Split(';');
			List<IDevice> deviceList = new List<IDevice>();

			foreach(var device in devices)
				deviceList.Add(Device.FromString(device));

			Payload = deviceList.ToArray();
		}
	}
}
