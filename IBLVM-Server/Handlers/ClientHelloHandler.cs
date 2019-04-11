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

namespace IBLVM_Server.Handlers
{
	class ClientHelloHandler : IPacketHandler
	{
		private readonly ECDiffieHellmanCng keyExchanger = new ECDiffieHellmanCng();
		private IPacketFactory packetFactory;

		public ClientHelloHandler(IPacketFactory packetFactory)
		{
			this.packetFactory = packetFactory;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.Hello)
			{
				if (socket.Status != (int)SocketStatus.Uninitialized)
					throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

				IPacket packet = packetFactory.CreateServerKeyResponse(keyExchanger.PublicKey.ToByteArray());
				SocketUtil.SendPacket(socket.GetSocketStream(), packet);

				socket.Status = (int)SocketStatus.ServerKeyResponsed;
				return true;
			}

			return false;
		}
	}
}
