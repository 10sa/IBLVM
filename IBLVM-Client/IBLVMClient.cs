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

using CryptoStream;

namespace IBLVM_Client
{
	public class IBLVMClient : IDisposable, IIBLVMSocket
	{
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly IPacketFactory packetFactory = new PacketFactroy();
		private readonly NetworkStream networkStream;
		private readonly byte[] socketBuffer;

		public IBLVMClient()
		{
			CryptoProvider.ECDiffieHellman.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
			socketBuffer = new byte[packetFactory.PacketSize * 2];
			CryptoProvider.ECDiffieHellman.HashAlgorithm = CngAlgorithm.Sha256;
			networkStream = new NetworkStream(socket);
		}

		#region Public methods
		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			Handshake();
		}

		private void Handshake()
		{
			socket.Send(packetFactory.CreateClientHello().GetPacketBytes());
			SocketUtil.ReceiveFull(networkStream, socketBuffer, packetFactory.PacketSize);
			IPacket header = packetFactory.ParseHeader(socketBuffer);

			if (header.Type == PacketType.ServerKeyResponse)
			{
				byte[] publicKey = SocketUtil.ReceiveFull(networkStream, header.GetPayloadSize());
				byte[] shareKey = CryptoProvider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

				CryptoProvider.CryptoStream = new CryptoMemoryStream(shareKey, shareKey);
				IPacket responsePacket = packetFactory.CreateClientKeyResponse(CryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
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

		public CryptoProvider CryptoProvider { get; set; }

		public NetworkStream GetSocketStream() => networkStream;
		#endregion
	}
}
