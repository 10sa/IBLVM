using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
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
		private AutoResetEvent deviceReceivedEvent = new AutoResetEvent(false);

		public event Action<DriveInformation[]> OnDrivesReceived = (a) => { };

		private readonly Action<IDevice> BroadcastDriveReqesting;

		public ManagerDrivesRequestHandler(Action<IDevice> broadcastDriveReqesting)
		{
			BroadcastDriveReqesting = broadcastDriveReqesting;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ManagerDrivesRequest)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), true);
				IPayload<IDevice> packet = socket.PacketFactory.CreateManagerDrivesRequest(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				BroadcastDriveReqesting(packet.Payload);
				deviceReceivedEvent.WaitOne();

				return true;
			}

			return false;
		}
	}
}
