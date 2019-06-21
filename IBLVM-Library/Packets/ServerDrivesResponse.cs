using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Packets
{
	public class ServerDrivesResponse : BasePacket, IPayload<ClientDrive[]>
	{
		public ClientDrive[] Payload { get; private set; }

		public ServerDrivesResponse(ClientDrive[] payload) : base(PacketType.ServerDrivesResponse)
		{
			Payload = payload;
		}

		public override int GetPayloadSize() => -1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] data = Encoding.UTF8.GetBytes(string.Join(";", (IEnumerable<ClientDrive>)Payload));
			buffer.Write(data, 0, data.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			string[] data = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			List<ClientDrive> driveInfo = new List<ClientDrive>();
			foreach (var str in data)
				driveInfo.Add(ClientDrive.FromString(str));

			Payload = driveInfo.ToArray();
		}
	}
}
