using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Library.Models;

namespace IBLVM_Client.Handlers
{
    class ServerBitLockerLockRequestHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockerLockRequest)
            {
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);

                IPayload<DriveInformation> packet = socket.PacketFactory.CreateServerBitLockerLockRequest(null);
                packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

                BitLocker bitLocker = BitLocker.GetVolume(packet.Payload.Name);
                bool isSuccess = true;
                try {
                    bitLocker.Lock(true);
                }
                catch (Exception)
                {
                    isSuccess = false;
                }

                IPayload<bool> result = socket.PacketFactory.CreateClientBitLockerCommandResponse(isSuccess);
                Utils.SendPacket(socket.SocketStream, result);

                return true;
            }

            return false;
        }
    }
}
