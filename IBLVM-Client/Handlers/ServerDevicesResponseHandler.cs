using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Client.Enums;
using IBLVM_Library.Models;

namespace IBLVM_Client.Handlers
{
	class ServerDevicesResponseHandler : IPacketHandler
	{
		public event Action<IDevice[]> OnDevicesReceived = (a) => { };

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerDevicesResponse)
			{
				Utils.PacketValidation(socket.Status, (int)SocketStatus.LoggedIn, header.GetPayloadSize(), false);
				IPayload<IDevice[]> packet = socket.PacketFactory.CreateServerDevicesResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				OnDevicesReceived(packet.Payload);
				return true;
			}

			return false;
		}
	}
}
