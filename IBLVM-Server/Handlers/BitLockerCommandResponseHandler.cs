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

namespace IBLVM_Server.Handlers
{
    class BitLockerCommandResponseHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ClientBitLockerCommandResponse)
            {
                if (socket.Status !=  (int)SocketStatus.LoggedIn)
                    throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

                if (header.GetPayloadSize() == 0)
                    throw new ProtocolViolationException("Protocol violation by empty payload.");

                // TO DO :: Handling message queue //

                return true;
            }

            return false;
        }
    }
}
