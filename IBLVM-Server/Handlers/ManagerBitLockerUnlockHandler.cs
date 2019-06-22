using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Handlers
{
	class ManagerBitLockerUnlockHandler : IPacketHandler
	{
		private IBroadcaster broadcaster;
		private IDeviceController deviceController;
		private ISession session;
		private CryptoProvider cryptor;

		public ManagerBitLockerUnlockHandler(IBroadcaster broadcaster, IDeviceController deviceController, ISession session, CryptoProvider cryptor)
		{
			this.broadcaster = broadcaster;
			this.deviceController = deviceController;
			this.session = session;
			this.cryptor = cryptor;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ManagerBitLockerUnlock)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);

				IPayload<ManagerBitLockerUnlock> packet = socket.PacketFactory.CreateManagerBitLockerUnlockRequest(null, null, cryptor);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				IDevice receiver = (from device in deviceController.GetUserDevices(session.Account.Id) where device.DeviceIP.Equals(packet.Payload.Drive.IP) select device).FirstOrDefault();
				if (receiver != null)
					Utils.SendPacket(socket.SocketStream, socket.PacketFactory.CreateServerBitLockerCommandResponse(broadcaster.RequestBitLockerUnlock(receiver, packet.Payload.Drive.Drive, packet.Payload.Password)));
				else
					Utils.SendPacket(socket.SocketStream, socket.PacketFactory.CreateServerBitLockerCommandResponse(false));

				return true;
			}

			return false;
		}
	}
}
