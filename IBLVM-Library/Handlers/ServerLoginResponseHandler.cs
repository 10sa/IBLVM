using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Exceptions;

namespace IBLVM_Library.Handlers
{
	public class ServerLoginResponseHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerLoginResponse)
			{
                Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.Connected, header.GetPayloadSize(), false);

                IPayload<bool> packet = socket.PacketFactory.CreateServerLoginResponse(false);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				if (!packet.Payload)
					throw new InvalidAuthorizationDataException();

				socket.Status = (int)ClientSocketStatus.LoggedIn;
				return true;
			}

			return false;
		}
	}
}
