using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.IO;

using IBLVM_Libaray.Interfaces;
using IBLVM_Libaray.Factories;
using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Models;

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
		private readonly byte[] socketBuffer;

		public IBLVMClient()
		{
			CryptoProvider.ECDiffieHellman = new ECDiffieHellmanCng();

			CryptoProvider.ECDiffieHellman.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
			socketBuffer = new byte[PacketFactory.PacketSize * 2];
			CryptoProvider.ECDiffieHellman.HashAlgorithm = CngAlgorithm.Sha256;
		}

		#region Public methods
		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			networkStream = new NetworkStream(socket);
			Handshake();
		}

		private void Handshake()
		{
			socket.Send(PacketFactory.CreateClientHello().GetPacketBytes());
			SocketUtil.ReceiveFull(networkStream, socketBuffer, PacketFactory.PacketSize);
			IPacket header = PacketFactory.ParseHeader(socketBuffer);

			if (header.Type == PacketType.ServerKeyResponse)
			{
				byte[] publicKey = SocketUtil.ReceiveFull(networkStream, header.GetPayloadSize());
				CryptoProvider.SharedKey = CryptoProvider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));
				CryptoProvider.CryptoStream = new CryptoMemoryStream(CryptoProvider.SharedKey);
				Array.Copy(CryptoProvider.SharedKey, CryptoProvider.CryptoStream.IV, CryptoProvider.CryptoStream.IV.Length);

				IPacket responsePacket = PacketFactory.CreateClientKeyResponse(CryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				SocketUtil.SendPacket(networkStream, responsePacket);
			}
			else
				throw new ProtocolViolationException("Received wrong header.");
		}
		#endregion

		#region IDispose implements
		public void Dispose()
		{
			CryptoProvider.CryptoStream.Dispose();
			CryptoProvider.ECDiffieHellman.Dispose();
			socket.Dispose();
		}
		#endregion

		#region IIBLVMSocket implements
		public int Status { get; set; }

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public NetworkStream GetSocketStream() => networkStream;
		#endregion
	}
}
