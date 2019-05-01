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
    public class IVChangeResponseHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.IVChangeResponse)
            {

                IPayload<bool> result = socket.PacketFactory.CreateIVChangeResposne(false);
                result.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

                if (!result.Payload)
                    throw new InvalidOperationException("IV Change request isn't accepted.");

                return true;
            }

            return false;
        }
    }
}
