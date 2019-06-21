using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library;
using IBLVM_Library.Handlers;
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
			chain.AddHandler(new ServerKeyResponseHandler());
			chain.AddHandler(new ServerLoginResponseHandler());
			chain.AddHandler(new IVChangeRequestHandler());
            chain.AddHandler(new IVChangeResponseHandler());
			chain.AddHandler(new BitLockerLockCommandHandler());
			chain.AddHandler(new BitLockerUnLockCommandHandler());
			chain.AddHandler(new ServerDrivesRequestHandler());
		}

		// Proxy
		public void DoHandle(IPacket packet) => chain.DoHandle(packet);
	}
}
