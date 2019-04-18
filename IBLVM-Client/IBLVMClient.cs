using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.IO;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Factories;
using IBLVM_Library;
using IBLVM_Library.Models;

using IBLVM_Util;
using IBLVM_Util.Interfaces;

using IBLVM_Client.Enums;

using SecureStream;

namespace IBLVM_Client
{
	public class IBLVMClient : IDisposable, IIBLVMSocket
	{
		public IPacketFactory PacketFactory { get; private set; } = new PacketFactroy();

		private readonly Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private NetworkStream networkStream;

		public IBLVMClient()
		{
			CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng();

			CryptoProvider.ECDiffieHellman.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
			CryptoProvider.ECDiffieHellman.HashAlgorithm = CngAlgorithm.Sha256;
		}

		#region Public methods
		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			networkStream = new NetworkStream(socket);

			Status = (int)SocketStatus.Handshaking;
			socket.Send(PacketFactory.CreateClientHello().GetPacketBytes());
		}

		public void Login(string id, string password)
		{
			if (Status != (int)SocketStatus.Connected)
				throw new InvalidOperationException("Not logged in!");

			Utils.SendPacket(networkStream, PacketFactory.CreateClientLoginRequest(id, password, CryptoProvider.CryptoStream));
		}
		#endregion

		#region IDispose implements
		public void Dispose()
		{
			CryptoProvider.Dispose();
			socket.Dispose();
		}
		#endregion

		#region IIBLVMSocket implements
		public int Status { get; set; } = (int)SocketStatus.Disconnected;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public NetworkStream GetSocketStream() => networkStream;
		#endregion
	}
}
