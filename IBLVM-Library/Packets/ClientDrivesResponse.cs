using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

namespace IBLVM_Library.Packets
{
    public class ClientDrivesResponse : BasePacket, IPayload<DriveInformation[]>
    {
        public ClientDrivesResponse(DriveInfo[] driveInfos) : base(PacketType.ClientDrivesResponse)
        {
			if (driveInfos != null)
				Payload = Array.ConvertAll(driveInfos, input => (DriveInformation)input);
        }

        public DriveInformation[] Payload { get; private set; }

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
            string serializedDrives = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize));

            List<DriveInformation> driveInfos = new List<DriveInformation>();

            foreach(var driveInfo in serializedDrives.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] infos = driveInfo.Split(',');
                driveInfos.Add(new DriveInformation(infos[0], infos[1], long.Parse(infos[2]), long.Parse(infos[3]), (DriveType)int.Parse(infos[4])));
            }

            Payload = driveInfos.ToArray();
        }
    }
}
