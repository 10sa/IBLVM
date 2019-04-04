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

using CryptoStream;

namespace IBLVM_Client
{
	public class IBLVMClient
	{
		private readonly IPacketFactory packetFactory = new PacketFactroy();
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly ECDiffieHellmanCng keyExchanger = new ECDiffieHellmanCng();
		private byte[] socketBuffer;
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
			IPacket header = packetFactory.Parse(socketBuffer);

			if (header.Type == PacketType.ServerKeySend)
			{
				byte[] publicKey = ReceiveFull(header.GetPayloadSize());
				byte[] shareKey = keyExchanger.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

				cryptoStream = new CryptoMemoryStream(shareKey, shareKey);
				IPacket responsePacket = packetFactory.CreateServerKeyResponse(keyExchanger.PublicKey.ToByteArray());
				SendPacket(responsePacket);
			}
			else
				throw new ProtocolViolationException("Received wrong header.");
		}

		private void SendPacket(IPacket packet)
		{
			socket.Send(packet.GetPacketBytes());
			if (packet.GetPayloadSize() > 0)
			{
				using (Stream stream = packet.GetPayloadStream())
				{
					int readedSize;
					while ((readedSize = stream.Read(socketBuffer, 0, socketBuffer.Length)) > 0)
						socket.Send(socketBuffer, readedSize, SocketFlags.None);
				}
			}
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
	}
}
