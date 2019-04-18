using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Util;
using IBLVM_Util.Interfaces;

using IBLVM_Client.Handlers;

using IBLVM_Library.Interfaces;

namespace IBLVM_Client
{
	class SocketHandlerChain
	{
		private readonly PacketHandlerChain chain;

		public SocketHandlerChain(IIBLVMSocket socket)
		{
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new IVExchangeHandler());
		}

		// Proxy
		public void DoHandle(IPacket packet) => chain.DoHandle(packet);
	}
}
