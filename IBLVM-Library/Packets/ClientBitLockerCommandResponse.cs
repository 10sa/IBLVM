using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

namespace IBLVM_Library.Packets
{
    public class ClientBitLockerCommandResponse : BaseActionResultPacket
    {
        public ClientBitLockerCommandResponse(bool isSuccess) : base(isSuccess, PacketType.ClientBitLockerCommandResponse) { }
    }
}
