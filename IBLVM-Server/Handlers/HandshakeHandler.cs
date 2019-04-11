using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;
using IBLVM_Util.Interfaces;
using IBLVM_Libaray.Models;

using IBLVM_Server.Enums;

using IBLVM_Libaray.Enums;

namespace IBLVM_Server.Handlers
{
	class HandshakeHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.Hello)
			{
				int payloadSize = header.GetPayloadSize();

			}

			return false;
		}
	}
}
