using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;
using IBLVM_Util.Interfaces;
using IBLVM_Libaray.Enums;

using CryptoStream;

namespace IBLVM_Client.Handlers
{
	class IVExchangeHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ExchangeInitializeVector)
			{
				socket.GetSocketStream();
				CryptoMemoryStream stream = socket.GetCryptoStream();


				return true;
			}

			return false;
		}
	}
}
