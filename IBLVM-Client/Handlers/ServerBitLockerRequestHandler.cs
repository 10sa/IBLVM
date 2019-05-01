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

namespace IBLVM_Client.Handlers
{
    class ServerBitLockerRequestHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockersRequest)
            {
                if (socket.Status != (int)SocketStatus.LoggedIn)
                    throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

                if (header.GetPayloadSize() > 0)
                    throw new ProtocolViolationException("Protocol violation by unreasonable payload.");

                IPacket bitlockers = socket.PacketFactory.CreateClientBitLockersResponse(BitLocker.GetVolumes());
                Utils.SendPacket(socket.GetSocketStream(), bitlockers);

                return true;
            }

            return false;
        }
    }
}
