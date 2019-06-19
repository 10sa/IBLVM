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

		public NetworkStream SocketStream { get; private set; }

		public Thread Receiver { get; private set; }

		public event Action<IDevice[]> OnDevicesReceived
		{
			add
			{
				chain.OnDevicesReceived += value;
			}
			remove
			{
				chain.OnDevicesReceived -= value;
			}
		}

		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly ManagerHandlerChain chain;
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
			SocketStream = new NetworkStream(socket);
			Receiver = new Thread(() =>
			{
				try
				{
					while (true)
					{
						Utils.ReadFull(SocketStream, buffer, PacketFactory.PacketSize);
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
			Utils.SendPacket(SocketStream, PacketFactory.CreateClientHello());
		}

		public void Login(string id, string password)
		{
			if (Status != (int)ClientSocketStatus.Connected)
				throw new InvalidOperationException("Not connected!");

			Utils.SendPacket(SocketStream, PacketFactory.CreateClientLoginRequest(id, password, ClientType.Manager, CryptoProvider.CryptoStream));
		}

		public void GetDeviceList()
		{
			if (Status != (int)ClientSocketStatus.LoggedIn)
				throw new InvalidOperationException("Not logged in!");

			Utils.SendPacket(SocketStream, PacketFactory.CreateManagerDevicesRequest());
		}

		public void GetBitLockerDrives(IDevice device)
		{
			if (Status != (int)ClientSocketStatus.LoggedIn)
				throw new InvalidOperationException("Not logged in!");

			Utils.SendPacket(SocketStream, PacketFactory.CreateManagerDrivesRequest(device));
		}

		public void Dispose()
		{
			CryptoProvider.Dispose();
			SocketStream.Dispose();
			socket.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
