﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using System.IO;
using IBLVM_Library.Models;

namespace IBLVM_Management.Handlers
{
	class ServerDrivesResponseHandler : IPacketHandler
	{
		public event Action<ClientDrive[]> OnDrivesReceived = (a) => {};

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerDrivesResponse)
			{
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn);

				IPayload<ClientDrive[]> packet = socket.PacketFactory.CreateServerDrivesResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				OnDrivesReceived(packet.Payload);
				return true;
			}

			return false;
		}
	}
}
