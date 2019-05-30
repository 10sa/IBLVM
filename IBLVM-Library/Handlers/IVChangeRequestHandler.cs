using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Args;

using SecureStream;

namespace IBLVM_Library.Handlers
{
	public class IVChangeRequestHandler : IPacketHandler
	{
		public event Action<IVExchangeAcceptEventArgs> IVExchangeAccpetEvent = (a) => { };

		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.IVChangeReqeust)
			{
                IPayload<byte[]> cryptoPacket = socket.PacketFactory.CreateIVChangeRequest(null);
                cryptoPacket.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());
                Array.Copy(cryptoPacket.Payload, socket.CryptoProvider.CryptoStream.IV, cryptoPacket.Payload.Length);

                // TO DO:: IV Changing accept event
                var resultPacket = socket.PacketFactory.CreateIVChangeResposne(true);
                Utils.SendPacket(socket.GetSocketStream(), resultPacket);

                return true;
			}

			return false;
		}
	}
}
