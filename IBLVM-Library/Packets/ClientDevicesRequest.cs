using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Library.Models;
using IBLVM_Library.Packets;

namespace IBLVM_Library.Packets
{
    public class ClientDevicesRequest : BasePacket
    {
        public ClientDevicesRequest() : base(PacketType.ClientDevicesRequest) { }
    }
}
