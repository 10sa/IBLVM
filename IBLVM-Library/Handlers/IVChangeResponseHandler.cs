using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;

namespace IBLVM_Library.Handlers
{
    class IVChangeResponseHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.IVChangeResponse)
            {
                
            }
        }
    }
}
