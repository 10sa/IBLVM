using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;

namespace IBLVM_Client.Handlers
{
    class ServerBitLockerRequestHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockersRequest)
            {
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), true);

                IPacket bitlockers = socket.PacketFactory.CreateClientBitLockersResponse(BitLocker.GetVolumes());
                Utils.SendPacket(socket.GetSocketStream(), bitlockers);

                return true;
            }

            return false;
        }
    }
}
