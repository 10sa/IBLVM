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

		public event Action<IAuthInfo> OnClientLoggedIn;

		public ServerHandlerChain(IIBLVMSocket socket, IAuthenticator session, IDeviceController deviceController, IPacketFactory packetFactory)
		{
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new ClientHelloHandler(packetFactory));
			chain.AddHandler(new ClientKeyResponseHandler());

			ClientLoginHandler clientLoginHandler = new ClientLoginHandler(session);
			chain.AddHandler(clientLoginHandler);
			clientLoginHandler.OnClientLoggedIn += OnClientLoggedIn;

            chain.AddHandler(new IVChangeRequestHandler());
            chain.AddHandler(new IVChangeResponseHandler());
			chain.AddHandler(new BitLockerCommandResponseHandler());
			chain.AddHandler(new ClientDevicesRequestHandler(deviceController, session));
        }

		public bool DoHandle(IPacket header) => chain.DoHandle(header);
	}
}
