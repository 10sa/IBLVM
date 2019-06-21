using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Packets
{
	public sealed class ClientHello : BasePacket
	{
		public ClientHello() : base(Enums.PacketType.ClientHello) { }

		public ClientHello(byte[] data, ref int offset) : base(data, ref offset) { }
	}
}
