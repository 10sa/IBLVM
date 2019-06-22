using System;
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

namespace IBLVM_Client.Handlers
{
    class ServerBitLockerUnlockRequestHandler : IPacketHandler
    {
        public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.ServerBitLockerUnlockRequest)
            {
				Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.LoggedIn, header.GetPayloadSize(), false);

				IPayload<ServerBitLockerUnlock> packet = socket.PacketFactory.CreateServerBitLockerUnlockRequest(null, null, socket.CryptoProvider);
                packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				DriveInformation volume = packet.Payload.Drive;

                BitLocker bitLocker = BitLocker.GetVolume(volume.Name);
                bool isSuccess = true;
                try
                {
                    bitLocker.Unlock(packet.Payload.Password);
                }
                catch (Exception)
                {
                    isSuccess = false;
                }

                IPayload<bool> result = socket.PacketFactory.CreateClientBitLockerCommandResponse(isSuccess);
                Utils.SendPacket(socket.SocketStream, result);
            }

            return false;
        }
    }
}
