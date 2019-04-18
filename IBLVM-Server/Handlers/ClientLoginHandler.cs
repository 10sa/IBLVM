using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Security.Cryptography;

using IBLVM_Library.Interfaces;
using IBLVM_Util.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Util;

using IBLVM_Server.Enums;
using IBLVM_Server.Interfaces;

using IBLVM_Library.Enums;

using SecureStream;

namespace IBLVM_Server.Handlers
{
	class ClientLoginHandler : IPacketHandler
	{
		private readonly IUserValidate userValidate;

		public ClientLoginHandler(IUserValidate userValidate)
		{
			this.userValidate = userValidate;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ClientLoginRequest)
			{
				if (socket.Status != (int)SocketStatus.Connected)
					throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

				IAuthentication packet = socket.PacketFactory.CreateClientLoginRequest(null, null, socket.CryptoProvider.CryptoStream);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				if (!userValidate.Validate(packet.Id, packet.Password))
					throw new ArgumentException("Invalid user data!");

				socket.Status = (int)SocketStatus.LoggedIn;
				return true;
			}

			return false;
		}
	}
}
