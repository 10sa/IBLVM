using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Server.Enums;

using IBLVM_Library.Models;
using IBLVM_Server.Interfaces;

namespace IBLVM_Server.Handlers
{
	class ManagerDevicesRequestHandler : IPacketHandler
	{
		private IDeviceController deviceController;
		private ISession session;

		public ManagerDevicesRequestHandler(IDeviceController deviceController, ISession session)
		{
			this.deviceController = deviceController;
			this.session = session;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ManagerDevicesRequest)
			{
				Utils.PacketValidation(socket.Status, (int)SocketStatus.LoggedIn, header.GetPayloadSize(), true);
				IPayload<IDevice[]> packet = socket.PacketFactory.CreateServerDevicesResponse(deviceController.GetUserDevices(session.Account.Id));

				Utils.SendPacket(socket.SocketStream, packet);
				return true;
			}

			return false;
		}
	}
}
