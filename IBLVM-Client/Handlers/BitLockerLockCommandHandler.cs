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
    class BitLockerLockCommandHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockerLockCommand)
            {
                if (socket.Status != (int)SocketStatus.LoggedIn)
                    throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

                if (header.GetPayloadSize() == 0)
                    throw new ProtocolViolationException("Protocol violation by empty payload.");

                IPayload<BitLockerVolume> packet = socket.PacketFactory.CreateBitLockerLockCommand(null);
                packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

                BitLocker bitLocker = BitLocker.GetVolume(packet.Payload.DriveLetter, packet.Payload.DeviceID);
                bool isSuccess = true;
                try {
                    bitLocker.Lock(true);
                }
                catch (Exception)
                {
                    isSuccess = false;
                }

                IPayload<bool> result = socket.PacketFactory.CreateClientBitLockerCommandResponse(isSuccess);
                Utils.SendPacket(socket.GetSocketStream(), result);

                return true;
            }

            return false;
        }
    }
}
