using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using System.IO;

namespace IBLVM_Libaray.Models
{
	public sealed class BitLockerList : BasePacket
	{
		private BitLockerVolume[] bitLockerVolumes;
		private byte[] serializedData;

		public BitLockerList(BitLockerVolume[] bitLockers) : base(PacketType.BitLockerList)
		{
			bitLockerVolumes = bitLockers;
			serializedData = Serialize();
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			serializedData = new byte[payloadSize];

			for (int readedSize = 0; payloadSize > readedSize;)
				readedSize += stream.Read(serializedData, readedSize, payloadSize - readedSize);

			bitLockerVolumes = Deserialize();
		}

		public override int GetPayloadSize()
		{
			return base.GetPayloadSize() + serializedData.Length;
		}

		private BitLockerVolume[] Deserialize()
		{
			string serializedString = Encoding.UTF8.GetString(serializedData);
			List<BitLockerVolume> volumes = new List<BitLockerVolume>();

			foreach (string volumeInfo in serializedString.Split(';'))
			{
				string[] data = volumeInfo.Split('/');
				volumes.Add(new BitLockerVolume(data[0], data[1]));
			}

			return volumes.ToArray();
		}

		private byte[] Serialize()
		{
			StringBuilder builder = new StringBuilder();
			foreach(var bitlocker in bitLockerVolumes)
			{
				builder.Append(bitlocker.DriveLetter);
				builder.Append("/");
				builder.Append(bitlocker.DeviceID);
				builder.Append(";");
			}

			return Encoding.UTF8.GetBytes(builder.ToString());
		}
	}
}
