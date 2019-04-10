using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Net.Sockets;
using System.Net;

namespace IBLVM_Server
{
	class IBLVMServer
	{
		public Thread ServerThread { get; private set; }

		private Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private List<ClientHandler> clientHandlers = new List<ClientHandler>();

		public void Bind(EndPoint localEndPoint) => serverSocket.Bind(localEndPoint);

		public void Listen(int backlog) => serverSocket.Listen(backlog);

		public void Start()
		{
			ServerThread = new Thread(() =>
			{
				Socket clientSocket = serverSocket.Accept();
				ClientHandler clientHandler = new ClientHandler(clientSocket);
				clientHandlers.Add(clientHandler);

				clientHandler.Start();
			});

			ServerThread.Start();
		}
	}
}
