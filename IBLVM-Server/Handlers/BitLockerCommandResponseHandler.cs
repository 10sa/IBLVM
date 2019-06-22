using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using System.Security.Cryptography;

using IBLVM_Library.Interfaces;

using IBLVM_Server.Enums;

using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Server.Models;

namespace IBLVM_Server.Handlers
{
    class BitLockerCommandResponseHandler : IPacketHandler
    {
		private MessageQueue messageQueue;

		public BitLockerCommandResponseHandler(MessageQueue messageQueue)
		{
			this.messageQueue = messageQueue;
		}

		public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ClientBitLockerCommandResponse)
            {
                Utils.PacketValidation(socket.Status, (int)SocketStatus.LoggedIn, header.GetPayloadSize(), false);
				IPayload<bool> packet = socket.PacketFactory.CreateClientBitLockerCommandResponse(false);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				messageQueue.Enqueue(packet.Type, packet.Payload);
                return true;
            }

            return false;
        }
    }
}
