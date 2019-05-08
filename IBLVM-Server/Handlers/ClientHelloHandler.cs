using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using System.Security.Cryptography;

using IBLVM_Library.Interfaces;

using IBLVM_Server.Enums;

using IBLVM_Library;
using IBLVM_Library.Enums;

namespace IBLVM_Server.Handlers
{
	class ClientHelloHandler : IPacketHandler
	{
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

                if (header.GetPayloadSize() > 0)
                    throw new ProtocolViolationException("Protocol violation by unreasonable payload.");

                socket.CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng();
				IPacket packet = packetFactory.CreateServerKeyResponse(socket.CryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				Utils.SendPacket(socket.GetSocketStream(), packet);

				socket.Status = (int)SocketStatus.ServerKeyResponsed;
				return true;
			}

			return false;
		}
	}
}
