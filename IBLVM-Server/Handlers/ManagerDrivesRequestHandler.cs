using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Interfaces;
using IBLVM_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBLVM_Server.Handlers
{
	class ManagerDrivesRequestHandler : IPacketHandler
	{
		private readonly AutoResetEvent deviceReceivedEvent = new AutoResetEvent(false);
		private readonly IBroadcaster broadcaster;

		public ManagerDrivesRequestHandler(IBroadcaster broadcaster)
		{
			this.broadcaster = broadcaster;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ManagerDrivesRequest)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);
				IPayload<IDevice> packet = socket.PacketFactory.CreateManagerDrivesRequest(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				Utils.SendPacket(socket.SocketStream, socket.PacketFactory.CreateServerDrivesResponse(broadcaster.RequestDrives(packet.Payload)));

				return true;
			}

			return false;
		}
	}
}
