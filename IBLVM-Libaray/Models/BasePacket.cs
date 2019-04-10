using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Interfaces;
using System.IO;

namespace IBLVM_Libaray.Models
{
	public class BasePacket : IPacket
	{
		public PacketType Type { get; private set; }

		byte[] IPacket.MagicBytes => MagicBytes;

		public static readonly byte[] MagicBytes = new byte[] { 0xDA, 0xAB, 0xBC, 0xCD };

		private BasePacket() { }

		public BasePacket(byte[] data, ref int offset)
		{
			if (!data.SequenceEqual(MagicBytes))
				throw new ArgumentException("Invalid MagicBytes!");

			offset += MagicBytes.Length;
			Type = (PacketType)BitConverter.ToUInt16(data, offset);
			offset += sizeof(uint);
		}

		protected BasePacket(PacketType type) => Type = type;

		protected void CreateBytes(Stream buffer)
		{
			WriteToStream(buffer, MagicBytes);
			WriteToStream(buffer, BitConverter.GetBytes(GetPayloadSize()));
			WriteToStream(buffer, BitConverter.GetBytes((ushort)Type));
		}

		protected static void WriteToStream(Stream stream, byte[] data)
		{
			stream.Write(data, 0, data.Length);
		}

		public byte[] GetPacketBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream(128))
			{
				CreateBytes(memoryStream);
				return memoryStream.ToArray();
			}
		}

		public virtual int GetPayloadSize() => 0;

		public static int GetHeaderSize() => MagicBytes.Length + sizeof(int) + sizeof(PacketType);

		public virtual Stream GetPayloadStream() => new MemoryStream(256);

		public virtual void ParsePayload(Stream stream) { }

		int IPacket.GetPayloadSize() => GetPayloadSize();
	}
}
