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
                IPayload<byte[]> packet = socket.PacketFactory.CreateIVChangeRequest(null);
                packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				IVExchangeAcceptEventArgs accept = new IVExchangeAcceptEventArgs();
				IVExchangeAccpetEvent(accept);

				if (accept.Accpet)
					Array.Copy(packet.Payload, socket.CryptoProvider.CryptoStream.IV, packet.Payload.Length);

				Utils.SendPacket(socket.GetSocketStream(), socket.PacketFactory.CreateIVChangeResposne(accept.Accpet));
				return true;
			}

			return false;
		}
	}
}
