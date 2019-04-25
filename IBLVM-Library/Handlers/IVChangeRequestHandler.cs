using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;

using SecureStream;

namespace IBLVM_Library.Handlers
{
	class IVChangeRequestHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.IVChangeReqeust)
			{
                ICryptoExchanger cryptoPacket = socket.PacketFactory.CreateIVChangeRequest(null);
                cryptoPacket.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());
                Array.Copy(cryptoPacket.Data, socket.CryptoProvider.CryptoStream.IV, cryptoPacket.Data.Length);

                // TO DO:: IV Changing accept event
                IActionResult resultPacket = socket.PacketFactory.CreateIVChangeResposne(true);
                Utils.SendPacket(socket.GetSocketStream(), resultPacket);

                return true;
			}

			return false;
		}
	}
}
