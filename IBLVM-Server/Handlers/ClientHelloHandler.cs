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
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ClientHello)
			{
                Utils.PacketValidation(socket.Status, (int)SocketStatus.Uninitialized, header.GetPayloadSize(), true);

                socket.CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng();
				IPacket packet = socket.PacketFactory.CreateServerKeyResponse(socket.CryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				Utils.SendPacket(socket.SocketStream, packet);

				socket.Status = (int)SocketStatus.ServerKeyResponsed;
				return true;
			}

			return false;
		}
	}
}
