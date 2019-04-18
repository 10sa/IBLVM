﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Factories;

using IBLVM_Library.Models;
using System.Net.Sockets;
using System.Net;

namespace IBLVM_Server
{
	public class IBLVMServer
	{
		public Thread ServerThread { get; private set; }

		private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private readonly IPacketFactory factory = new PacketFactroy();

		public void Bind(EndPoint localEndPoint) => serverSocket.Bind(localEndPoint);

		public void Listen(int backlog) => serverSocket.Listen(backlog);

		public void Start()
		{
			ServerThread = new Thread(() =>
			{
				while(true)
				{
					Socket clientSocket = serverSocket.Accept();
					ClientHandler clientHandler = new ClientHandler(clientSocket, factory);
					clientHandler.Start();
				}
			});

			ServerThread.Start();
		}
	}
}
