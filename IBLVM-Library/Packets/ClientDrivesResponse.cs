using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Packets
{
    public class ClientDrivesResponse : BasePacket, IPayload<DriveInfomation[]>
    {
        public ClientDrivesResponse(DriveInfo[] driveInfos) : base(PacketType.ClientDrivesResponse)
        {
            Payload = Array.ConvertAll(driveInfos, input => (DriveInfomation)input);
        }

        public DriveInfomation[] Payload { get; private set; }

        public override int GetPayloadSize()
        {
            return base.GetPayloadSize();
        }

        public override Stream GetPayloadStream()
        {
            Stream buffer = base.GetPayloadStream();
            StringBuilder builder = new StringBuilder();
            foreach (var drive in Payload)
            {
                builder.Append(drive.Name + ",");
                builder.Append(string.Format("\"{0}\",", drive.VolumeLabel));
                builder.Append(drive.TotalSize + ",");
                builder.Append(drive.TotalFreeSpace + ",");
                builder.Append(drive.DriveType + ";");

                WriteToStream(buffer, Encoding.UTF8.GetBytes(builder.ToString()));
                builder.Clear();
            }

            return buffer;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);
            string serializedDrives = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize));

            List<DriveInfomation> driveInfos = new List<DriveInfomation>();

            foreach(var driveInfo in serializedDrives.Split(';'))
            {
                string[] infos = driveInfo.Split(',');
                driveInfos.Add(new DriveInfomation(infos[0], infos[1], long.Parse(infos[2]), long.Parse(infos[3]), (DriveType)int.Parse(infos[4])));
            }

            Payload = driveInfos.ToArray();
        }
    }
}
