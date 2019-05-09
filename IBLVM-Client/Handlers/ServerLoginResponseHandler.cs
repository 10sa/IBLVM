using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Exceptions;
using IBLVM_Client.Enums;

using IBLVM_Library;

namespace IBLVM_Client.Handlers
{
	class ServerLoginResponseHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerLoginResponse)
			{
                Utils.PacketValidation(socket.Status, (int)SocketStatus.Connected, header.GetPayloadSize());

                IPayload<bool> packet = socket.PacketFactory.CreateServerLoginResponse(false);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				if (!packet.Payload)
					throw new InvalidAuthorizationDataException();

				socket.Status = (int)SocketStatus.LoggedIn;
				return true;
			}

			return false;
		}
	}
}
