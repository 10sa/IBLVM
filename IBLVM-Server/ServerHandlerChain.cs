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

namespace IBLVM_Server
{
	class ServerHandlerChain
	{
		private readonly PacketHandlerChain chain;
		private readonly ClientLoginHandler clientLoginHandler;

		public event Action<IAuthInfo> OnClientLoggedIn
		{
			add {
				clientLoginHandler.OnClientLoggedIn += value;
			}
			remove {
				clientLoginHandler.OnClientLoggedIn -= value;
			}
		}

		public ServerHandlerChain(IIBLVMSocket socket, ISession session, IDeviceController deviceController, IPacketFactory packetFactory)
		{
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new ClientHelloHandler(packetFactory));
			chain.AddHandler(new ClientKeyResponseHandler());

			clientLoginHandler = new ClientLoginHandler(session);
			chain.AddHandler(clientLoginHandler);

            chain.AddHandler(new IVChangeRequestHandler());
            chain.AddHandler(new IVChangeResponseHandler());
			chain.AddHandler(new BitLockerCommandResponseHandler());
			chain.AddHandler(new ManagerDevicesRequestHandler(deviceController, session));
        }

		public bool DoHandle(IPacket header) => chain.DoHandle(header);
	}
}
