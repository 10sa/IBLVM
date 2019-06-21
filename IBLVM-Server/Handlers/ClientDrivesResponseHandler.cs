using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Handlers
{
	class ClientDrivesResponseHandler : IPacketHandler
	{
		private readonly MessageQueue messageQueue;

		public ClientDrivesResponseHandler(MessageQueue messageQueue)
		{
			this.messageQueue = messageQueue;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ClientDrivesResponse)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn);
				IPayload<DriveInformation[]> packet = socket.PacketFactory.CreateClientDrivesResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				messageQueue.Enqueue(header.Type, packet.Payload);
				return true;
			}

			return false;
		}
	}
}
