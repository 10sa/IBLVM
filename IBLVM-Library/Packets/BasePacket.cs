using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using System.IO;

namespace IBLVM_Library.Packets
{
	public class BasePacket : IPacket
	{
		public PacketType Type { get; private set; }

        public static readonly byte[] MagicBytes = new byte[] { 0xDA, 0xAB, 0xBC, 0xCD };

        private int payloadSize = 0;

		private BasePacket() { }

		public BasePacket(byte[] data, ref int offset)
		{
			for(int i = 0; i < MagicBytes.Length; i++)
			{
				if (data[i] != MagicBytes[i])
					throw new ArgumentException("Invalid MagicBytes!");
			}
			offset += MagicBytes.Length;

			payloadSize = BitConverter.ToInt32(data, offset);
			offset += sizeof(int);

			Type = (PacketType)BitConverter.ToInt16(data, offset);
			offset += sizeof(short);
		}

		protected BasePacket(PacketType type) => Type = type;

		protected void CreateBytes(Stream buffer)
		{
			WriteToStream(buffer, MagicBytes);
			WriteToStream(buffer, BitConverter.GetBytes(GetPayloadSize()));
			WriteToStream(buffer, BitConverter.GetBytes((short)Type));
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

		public virtual int GetPayloadSize() => payloadSize;

		public void OverridePayloadSize(int size) => payloadSize = size;

		public static int GetHeaderSize() => MagicBytes.Length + sizeof(int) + sizeof(PacketType);

		public virtual Stream GetPayloadStream() => new MemoryStream(256);

		public virtual void ParsePayload(int payloadSize, Stream stream)
		{
			// Re-define temp size //
			this.payloadSize = 0;
		}

		int IPacket.GetPayloadSize() => GetPayloadSize();
	}
}
