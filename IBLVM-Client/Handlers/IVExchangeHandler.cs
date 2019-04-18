using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;

namespace IBLVM_Client.Handlers
{
	class IVExchangeHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ExchangeInitializeVector)
			{
				socket.GetSocketStream();
				CryptoMemoryStream stream = socket.CryptoProvider.CryptoStream;


				return true;
			}

			return false;
		}
	}
}
