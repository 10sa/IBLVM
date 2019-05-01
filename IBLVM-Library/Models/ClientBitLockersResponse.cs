using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

using System.IO;


namespace IBLVM_Library.Models
{
	public sealed class ClientBitLockersResponse : BasePacket, IPayload<BitLockerVolume[]>
	{
		public BitLockerVolume[] Payload { get; private set; }
		private byte[] serializedData;

		public ClientBitLockersResponse(BitLockerVolume[] bitLockers) : base(PacketType.ClientBitLockersResponse)
		{
			Payload = bitLockers;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			serializedData = new byte[payloadSize];

			Utils.ReadFull(stream, payloadSize);

			string serializedString = Encoding.UTF8.GetString(serializedData);
			List<BitLockerVolume> volumes = new List<BitLockerVolume>();

			foreach (string volumeInfo in serializedString.Split(';'))
			{
				string[] data = volumeInfo.Split(',');
				volumes.Add(new BitLockerVolume(data[0], data[1]));
			}

			Payload = volumes.ToArray();
		}

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			StringBuilder builder = new StringBuilder();
			foreach (var bitlocker in Payload)
			{
				builder.Append(bitlocker.DriveLetter);
				builder.Append(",");
				builder.Append(bitlocker.DeviceID);
				builder.Append(";");
			}

			serializedData = Encoding.UTF8.GetBytes(builder.ToString());
			buffer.Write(serializedData, 0, serializedData.Length);

			return buffer;
		}

		public override int GetPayloadSize()
		{
			if (base.GetPayloadSize() > 0)
				return base.GetPayloadSize();
			else
				return -1;
		}
	}
}
