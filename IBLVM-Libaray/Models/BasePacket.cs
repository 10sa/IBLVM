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
	public abstract class BasePacket : IPacket
	{
		public PacketType Type { get; private set; }

		public static readonly byte[] MagicBytes = new byte[] { 0xDA, 0xAB, 0xBC, 0xCD };

		private BasePacket() { }

		protected BasePacket(PacketType type) => Type = type;

		protected virtual void CreateBytes(Stream buffer)
		{
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

		public virtual int GetPacketSize()
		{
			return MagicBytes.Length + sizeof(PacketType);
		}

		public abstract Stream GetPayloadStream();
	}
}
