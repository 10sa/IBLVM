using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;

namespace IBLVM_Library.Packets
{
    public class ServerDrivesRequest : BasePacket
    {
        public ServerDrivesRequest() : base(PacketType.ServerDrivesRequest) { }
    }
}
