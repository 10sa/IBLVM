using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Management.Handlers
{
	class ServerBitLockerCommandResponseHandler : IPacketHandler
	{
		public event Action<bool> OnBitLockerCommandResponseReceived = (a) => { };

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerBitLockerCommandResponse)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);
				IPayload<bool> packet = socket.PacketFactory.CreateServerBitLockerCommandResponse(false);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				OnBitLockerCommandResponseReceived(packet.Payload);
				return true;
			}

			return false;
		}
	}
}
