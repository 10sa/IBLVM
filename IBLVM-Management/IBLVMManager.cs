using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Library.Factories;
using IBLVM_Library.Enums;
using IBLVM_Library;
using System.Security.Cryptography;

namespace IBLVM_Management
{
	public class IBLVMManager : IIBLVMSocket, IDisposable
	{
		public int Status { get; set; } = (int)ClientSocketStatus.Disconnected;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public IPacketFactory PacketFactory { get; private set; } = new PacketFactroy();

		public Thread Receiver { get; private set; }

		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private NetworkStream networkStream;
		private ManagerHandlerChain chain;
		private readonly byte[] buffer;

		public IBLVMManager()
		{
			CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng
			{
				KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
				HashAlgorithm = CngAlgorithm.Sha256
			};

			buffer = new byte[PacketFactory.PacketSize * 2];
			chain = new ManagerHandlerChain(this);
		}

		public void Conncet(IPEndPoint endPoint)
		{
			socket.Connect(endPoint);
			networkStream = new NetworkStream(socket);
			Receiver = new Thread(() =>
			{
				try
				{
					while (true)
					{
						Utils.ReadFull(networkStream, buffer, PacketFactory.PacketSize);
						IPacket header = PacketFactory.ParseHeader(buffer);
						chain.DoHandle(header);
					}
				}
				catch (Exception)
				{
					if (socket.Connected)
						throw;

					Dispose();
				}

			});
			Receiver.Start();

			Status = (int)ClientSocketStatus.Handshaking;
			Utils.SendPacket(networkStream, PacketFactory.CreateClientHello());
		}

		public void Login(string id, string password)
		{
			if (Status != (int)ClientSocketStatus.Connected)
				throw new InvalidOperationException("Not connected!");

			Utils.SendPacket(networkStream, PacketFactory.CreateClientLoginRequest(id, password, ClientType.Manager, CryptoProvider.CryptoStream));
		}

		public NetworkStream SocketStream => networkStream;

		public void Dispose()
		{
			CryptoProvider.Dispose();
			networkStream.Dispose();
			socket.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
