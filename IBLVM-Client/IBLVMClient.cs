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

using IBLVM_Util;
using IBLVM_Util.Interfaces;

using IBLVM_Client.Enums;

using CryptoStream;

namespace IBLVM_Client
{
	public class IBLVMClient : IDisposable, IIBLVMSocket
	{
		public SocketStatus Status { get; private set; }

		private readonly IPacketFactory packetFactory = new PacketFactroy();
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly ECDiffieHellmanCng keyExchanger = new ECDiffieHellmanCng();
		private readonly byte[] socketBuffer;
		private CryptoMemoryStream cryptoStream;

		public IBLVMClient()
		{
			socketBuffer = new byte[packetFactory.PacketSize * 2];
			keyExchanger.HashAlgorithm = CngAlgorithm.Sha256;
			keyExchanger.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
		}

		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			Handshake();
		}

		private void Handshake()
		{
			socket.Send(packetFactory.CreateClientHello().GetPacketBytes());
			ReceiveFull(socketBuffer, packetFactory.PacketSize);
			IPacket header = packetFactory.ParseHeader(socketBuffer);

			if (header.Type == PacketType.ServerKeySend)
			{
				byte[] publicKey = ReceiveFull(header.GetPayloadSize());
				byte[] shareKey = keyExchanger.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

				cryptoStream = new CryptoMemoryStream(shareKey, shareKey);
				IPacket responsePacket = packetFactory.CreateServerKeyResponse(keyExchanger.PublicKey.ToByteArray());
				SocketUtil.SendPacket(socket, responsePacket);
			}
			else
				throw new ProtocolViolationException("Received wrong header.");
		}

		private byte[] ReceiveFull(int size)
		{
			byte[] buffer = new byte[size];
			ReceiveFull(buffer, size);

			return buffer;
		}

		private void ReceiveFull(byte[] buffer, int size)
		{
			for (int i = 0; i < size;)
				i += socket.Receive(buffer, i, size - i, SocketFlags.None);
		}

		public void Dispose()
		{
			cryptoStream.Dispose();
			keyExchanger.Dispose();
			socket.Dispose();
		}

		public void SetSocketStatus(int status) => this.Status = (SocketStatus)status;
	}
}
