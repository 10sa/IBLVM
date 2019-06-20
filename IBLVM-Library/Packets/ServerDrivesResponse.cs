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
	public class ServerDrivesResponse : BasePacket, IPayload<DriveInformation[]>
	{
		public DriveInformation[] Payload { get; private set; }

		public ServerDrivesResponse(DriveInformation[] payload) : base(PacketType.ServerDrivesResponse)
		{
			Payload = payload;
		}

		public override int GetPayloadSize() => -1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] data = Encoding.UTF8.GetBytes(string.Join(";", (IEnumerable<DriveInformation>)Payload));
			buffer.Write(data, 0, data.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			string[] data = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize)).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			List<DriveInformation> driveInfo = new List<DriveInformation>();
			foreach (var str in data)
				driveInfo.Add(DriveInformation.FromString(str));

			Payload = driveInfo.ToArray();
		}
	}
}
