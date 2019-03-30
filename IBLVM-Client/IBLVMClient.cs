using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

using IBLVM_Libaray.Interfaces;
using IBLVM_Libaray.Factories;
using IBLVM_Libaray.Enums;

using CryptoMemoryStream.IO;

namespace IBLVM_Client
{
	public class IBLVMClient
	{
		private IPacketFactory packetFactory = new PacketFactroy();
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly byte[] socketBuffer = new byte[256];

		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			Handshake();
		}

		private void Handshake()
		{
			socket.Send(packetFactory.CreateClientHello().GetPacketBytes());
			socket.Receive(socketBuffer, 0, packetFactory.PacketSize, SocketFlags.None);

			if (socketBuffer.SequenceEqual(packetFactory.MagicBytes) &&
					(PacketType)BitConverter.ToUInt16(socketBuffer, packetFactory.MagicBytes.Length) == PacketType.ServerKeySend)
				socket.Send(packetFactory.CreateServerKeyResponse().GetPacketBytes());
			else
				throw new ProtocolViolationException("Received wrong header.");
		}
	}
}
