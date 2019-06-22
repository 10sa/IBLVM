using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Handlers
{
	class ManagerBitLockerLockHandler : IPacketHandler
	{
		private IDeviceController deviceController;
		private ISession session;
		private IBroadcaster broadcaster;

		public ManagerBitLockerLockHandler(IDeviceController deviceController, ISession session, IBroadcaster broadcaster)
		{
			this.deviceController = deviceController;
			this.session = session;
			this.broadcaster = broadcaster;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ManagerBitLockerLock)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);
				IPayload<ClientDrive> packet = socket.PacketFactory.CreateManagerBitLockerLockRequest(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				IDevice receiver = (from device in deviceController.GetUserDevices(session.Account.Id) where device.DeviceIP == packet.Payload.IP select device).FirstOrDefault();
				if (receiver != null)
					Utils.SendPacket(socket.SocketStream, socket.PacketFactory.CreateServerBitLockerCommandResponse(broadcaster.RequestBitLockerLock(receiver, packet.Payload.Drive)));
				else
					Utils.SendPacket(socket.SocketStream, socket.PacketFactory.CreateServerBitLockerCommandResponse(false));

				return true;
			}

			return false;
		}
	}
}
