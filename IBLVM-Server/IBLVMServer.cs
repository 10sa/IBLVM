using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Factories;

using IBLVM_Server.Interfaces;

using System.Net.Sockets;
using System.Net;

namespace IBLVM_Server
{
	public sealed class IBLVMServer : IDisposable
	{
		public Thread ServerThread { get; private set; }

		public readonly IUserValidate userValidate;

		private readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private readonly List<ClientHandler> clientHandlers = new List<ClientHandler>();
		private readonly IPacketFactory factory = new PacketFactroy();

		public IBLVMServer(IUserValidate userValidate)
		{
			this.userValidate = userValidate;
		}

		public void Bind(EndPoint localEndPoint) => serverSocket.Bind(localEndPoint);

		public void Listen(int backlog) => serverSocket.Listen(backlog);

		public void Start()
		{
            ServerThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Socket clientSocket = serverSocket.Accept();
                        ClientHandler clientHandler = new ClientHandler(clientSocket, userValidate, factory);
                        clientHandlers.Add(clientHandler);
                        clientHandler.OnHandlerDisposed += OnClientDisconnected;

                        clientHandler.Start();
                    }
                    catch (ThreadAbortException)
                    {
						return;
                    }
                }
            })
            {
                Name = "IBLVM Server main thread"
            };
            ServerThread.Start();
		}

		private void OnClientDisconnected(ClientHandler handler)
		{
			clientHandlers.Remove(handler);
		}

        public void Dispose()
        {
            for (int i = 0; i < clientHandlers.Count; i++)
                clientHandlers[i].Dispose();

			serverSocket.Dispose();
			ServerThread.Abort();
            GC.SuppressFinalize(this);
        }
    }
}
