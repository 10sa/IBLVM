using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;
using IBLVM_Libaray.Models;

namespace IBLVM_Libaray.Factories
{
	public class PacketFactroy : IPacketFactory
	{
		public byte[] MagicBytes => BasePacket.MagicBytes;

		public int PacketSize => BasePacket.GetHeaderSize();

		public IPacket CreateClientHello()
		{
			return new ClientHello();
		}

		public IPacket CreateServerKeyResponse(byte[] cryptoKey)
		{
			return new ServerKeyResponse(cryptoKey);
		}
	}
}
