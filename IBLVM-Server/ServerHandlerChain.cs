using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Interfaces;

using IBLVM_Util.Interfaces;
using IBLVM_Util;

using IBLVM_Server.Handlers;

namespace IBLVM_Server
{
	class ServerHandlerChain
	{
		private PacketHandlerChain chain;

		public ServerHandlerChain(IIBLVMSocket socket, IPacketFactory packetFactory)
		{
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new ClientHelloHandler(packetFactory));
			chain.AddHandler(new ClientKeyResponseHandler());
		}

		public bool DoHandle(IPacket header) => chain.DoHandle(header);
	}
}
