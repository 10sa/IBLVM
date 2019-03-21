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
	abstract class BasePacket : IPacket
	{
		public PacketType Type { get; private set; }

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

		public abstract Stream GetPayloadStream();
	}
}
