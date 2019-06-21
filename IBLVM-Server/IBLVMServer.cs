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
using IBLVM_Server.Models;
using IBLVM_Server.Args;
using IBLVM_Library.Models;

namespace IBLVM_Server
{
	public sealed class IBLVMServer : IDisposable, IServer
	{
		public Thread ServerThread { get; private set; }

		public ISession Session { get; private set; }

		public IDeviceController DeviceController { get; private set; } = new DeviceController();

		public IPacketFactory PacketFactory { get; private set; } = new PacketFactroy();

		private readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private readonly List<ClientHandler> clientHandlers = new List<ClientHandler>();
		private readonly Broadcast broadcast = new Broadcast();

		public IBLVMServer(ISession session)
		{
			this.Session = session;
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
                        ClientHandler clientHandler = new ClientHandler(clientSocket, this, broadcast);
                        clientHandlers.Add(clientHandler);
                        clientHandler.OnHandlerDisposed += OnClientDisconnected;

                        clientHandler.Start();
                    }
                    catch (Exception)
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

		private class Broadcast : IBroadcaster
		{
			public event Action<DrivesRequestEventArgs> BroadcastDrivesRequest;

			public ClientDrive[] RequestDrives(IDevice device)
			{
				DrivesRequestEventArgs args = new DrivesRequestEventArgs(device);
				BroadcastDrivesRequest(args);

				return args.Drives.ToArray();
			}
		}
	}
}
