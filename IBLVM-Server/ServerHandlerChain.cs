using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Handlers;
using IBLVM_Library;

using IBLVM_Server.Handlers;
using IBLVM_Server.Interfaces;
using IBLVM_Server.Models;

namespace IBLVM_Server
{
	class ServerHandlerChain
	{
		private readonly ClientLoginHandler clientLoginHandler;
		private readonly PacketHandlerChain chain;

		public event Action<IAuthInfo> OnClientLoggedIn
		{
			add {
				clientLoginHandler.OnClientLoggedIn += value;
			}
			remove {
				clientLoginHandler.OnClientLoggedIn -= value;
			}
		}

		public ServerHandlerChain(IIBLVMSocket socket, MessageQueue messageQueue, IServer server, IBroadcaster broadcaster)
		{
		
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new ClientHelloHandler());
			chain.AddHandler(new ClientKeyResponseHandler());

			clientLoginHandler = new ClientLoginHandler(server.Session);
			chain.AddHandler(clientLoginHandler);

            chain.AddHandler(new IVChangeRequestHandler());
            chain.AddHandler(new IVChangeResponseHandler());
			chain.AddHandler(new BitLockerCommandResponseHandler(messageQueue));
			chain.AddHandler(new ClientDrivesResponseHandler(messageQueue));
			chain.AddHandler(new ManagerDevicesRequestHandler(server.DeviceController, server.Session));
			chain.AddHandler(new ManagerDrivesRequestHandler(broadcaster));
			chain.AddHandler(new ManagerBitLockerLockHandler(server.DeviceController, server.Session, broadcaster));
        }

		public bool DoHandle(IPacket header) => chain.DoHandle(header);
	}
}
