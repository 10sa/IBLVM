using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using System.IO;

namespace IBLVM_Libaray.Models
{
	public class BitLockerList : BasePacket
	{
		private BitLockerVolume[] bitLockerVolumes;
		private byte[] serializedData;

		public BitLockerList(BitLockerVolume[] bitLockers) : base(PacketType.BitLockerList)
		{
			bitLockerVolumes = bitLockers;
			serializedData = SerializeVolumes();
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			serializedData = new byte[payloadSize];

			int readedSize = 0;
			for (; payloadSize > readedSize;)
				readedSize += stream.Read(serializedData, readedSize, payloadSize - readedSize);

			string serializedString = Encoding.UTF8.GetString(serializedData);
			List<BitLockerVolume> volumes = new List<BitLockerVolume>();

			foreach (string volumeInfo in serializedString.Split(';'))
			{
				string[] data = volumeInfo.Split('/');
				volumes.Add(new BitLockerVolume(data[0], data[1]));
			}

			bitLockerVolumes = volumes.ToArray();
		}

		public override int GetPayloadSize()
		{
			return base.GetPayloadSize() + serializedData.Length;
		}

		private byte[] SerializeVolumes()
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
