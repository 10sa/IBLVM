using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Client.Handlers
{
	class ServerDrivesRequestHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerDrivesRequest)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), true);
				IPayload<DriveInformation[]> packet = socket.PacketFactory.CreateClientDrivesResponse(DriveInfo.GetDrives());

				Utils.SendPacket(socket.SocketStream, packet);
				return true;
			}

			return false;
		}
	}
}
