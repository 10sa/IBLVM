using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Security.Cryptography;

using IBLVM_Libaray.Interfaces;
using IBLVM_Util.Interfaces;
using IBLVM_Libaray.Models;
using IBLVM_Util;

using IBLVM_Server.Enums;

using IBLVM_Libaray.Enums;

using SecureStream;

namespace IBLVM_Server.Handlers
{
	class ClientLoginHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ClientLoginRequest)
			{
				if (socket.Status != (int)SocketStatus.Connected)
					throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

				

				return true;
			}

			return false;
		}
	}
}
