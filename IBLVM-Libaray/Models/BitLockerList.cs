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
		private BitLocker[] bitLockerVolumes;
		private byte[] serializedData;

		public BitLockerList(BitLocker[] bitLockers) : base(PacketType.BitLockerList)
		{
			bitLockerVolumes = bitLockers;
			SerializeVolumes();
		}

		public override void ParsePayload(Stream stream)
		{
			base.ParsePayload(stream);
			
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
				builder.Append("\"");
				builder.Append(bitlocker.DriveLetter);
				builder.Append(bitlocker.DeviceID);
				builder.Append("\";");
			}

			return Encoding.UTF8.GetBytes(builder.ToString());
		}
	}
}
