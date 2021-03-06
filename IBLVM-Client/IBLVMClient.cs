﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Factories;
using IBLVM_Library;
using IBLVM_Library.Enums;
using IBLVM_Library.Models;

namespace IBLVM_Client
{
	public sealed class IBLVMClient : IDisposable, IIBLVMSocket
	{
		public IPacketFactory PacketFactory { get; private set; } = new PacketFactroy();

		public Thread Receiver { get; private set; }

		private readonly Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private readonly RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
		private readonly SocketHandlerChain chain;
		private readonly byte[] buffer;

		private NetworkStream networkStream;

		public IBLVMClient()
		{
			CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng
			{
				KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
				HashAlgorithm = CngAlgorithm.Sha256
			};

			buffer = new byte[PacketFactory.PacketSize * 2];
			chain = new SocketHandlerChain(this);
		}

		#region Public methods
		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
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
				catch (SocketException)
				{
					if (socket.Connected)
						throw;
				}
				catch (IOException e)
				{
					if (e.InnerException != null && e.InnerException.GetType() != typeof(SocketException) && !socket.Connected)
						return;
				}
                finally
				{
					Dispose();
				}
                
			});
			Receiver.Name = "IBLVM Client Receiver";
			Receiver.Start();

			Status = (int)ClientSocketStatus.Handshaking;
			Utils.SendPacket(SocketStream, PacketFactory.CreateClientHello());
		}

		public void Login(string id, string password)
		{
			if (Status != (int)ClientSocketStatus.Connected)
				throw new InvalidOperationException("Not connected!");

			Utils.SendPacket(networkStream, PacketFactory.CreateClientLoginRequest(id, password, 0, CryptoProvider.CryptoStream));
		}

		public void ExchangeIV()
		{
			byte[] iv = new byte[CryptoProvider.CryptoStream.IV.Length];
			rngProvider.GetBytes(iv);

			Utils.ExchangeIV(this, iv);
		}
		#endregion

		#region IDispose implements
		public void Dispose()
		{
			CryptoProvider.Dispose();
            networkStream.Dispose();
			socket.Dispose();
            GC.SuppressFinalize(this);
		}
		#endregion

		#region IIBLVMSocket implements
		public int Status { get; set; } = (int)ClientSocketStatus.Disconnected;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public NetworkStream SocketStream => networkStream;
		#endregion
	}
}
