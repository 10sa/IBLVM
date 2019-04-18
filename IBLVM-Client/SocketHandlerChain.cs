using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library;
using IBLVM_Library.Interfaces;

using IBLVM_Client.Handlers;

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
