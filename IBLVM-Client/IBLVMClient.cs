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
		private byte[] socketBuffer = new byte[256];
		private CryptoMemoryStream cryptoStream;

		public IBLVMClient()
		{
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

			if (socketBuffer.SequenceEqual(packetFactory.MagicBytes) && 
				(PacketType)BitConverter.ToUInt16(socketBuffer, packetFactory.MagicBytes.Length) == PacketType.ServerKeySend)
			{
				ReceiveFull(socketBuffer, sizeof(int));
				int keySize = BitConverter.ToInt32(socketBuffer, 0);

				byte[] publicKey = ReceiveFull(keySize);
				byte[] shareKey = keyExchanger.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

				cryptoStream = new CryptoMemoryStream(shareKey, shareKey);
				socket.Send(packetFactory.CreateServerKeyResponse(keyExchanger.PublicKey.ToByteArray()).GetPacketBytes());
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
	}
}
