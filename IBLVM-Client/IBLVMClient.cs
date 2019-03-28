using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

using IBLVM_Libaray.Models;
using IBLVM_Libaray.Enums;

namespace IBLVM_Client
{
	public class IBLVMClient
	{
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private readonly byte[] socketBuffer = new byte[256];

		public void Connect(IPEndPoint remoteEndPoint)
		{
			socket.Connect(remoteEndPoint);
			Handshake();
		}

		private void Handshake()
		{
			socket.Send(new HelloRequestPacket().GetPacketBytes());
			socket.Receive(socketBuffer, 0, BasePacket.GetPacketSize(), SocketFlags.None);

			if (socketBuffer.SequenceEqual(BasePacket.MagicBytes) &&
					(PacketType)BitConverter.ToUInt16(socketBuffer, BasePacket.MagicBytes.Length) == PacketType.Ack)
				socket.Send(new HelloResponsePacket().GetPacketBytes());
			else
				throw new ProtocolViolationException("Received wrong header.");
		}
	}
}
