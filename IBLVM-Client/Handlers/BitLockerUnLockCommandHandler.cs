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
    class BitLockerUnLockCommandHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockerUnlockCommand)
            {
                if (socket.Status != (int)SocketStatus.LoggedIn)
                    throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

                if (header.GetPayloadSize() == 0)
                    throw new ProtocolViolationException("Protocol violation by empty payload.");

                IPayload<string> payload = socket.PacketFactory.CreateBitLockerUnlockCommand(null, socket.CryptoProvider.CryptoStream);
                payload.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

                
            }

            return false;
        }
    }
}
