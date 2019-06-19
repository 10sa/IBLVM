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
                Utils.PacketValidation(socket.Status, (int)SocketStatus.LoggedIn, header.GetPayloadSize(), false);

                // TODO: Handling message queue

                return true;
            }

            return false;
        }
    }
}
