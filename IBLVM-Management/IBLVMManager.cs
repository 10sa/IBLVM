using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Library.Factories;
using System.Net;
using IBLVM_Management.Enums;

namespace IBLVM_Management
{
	public class IBLVMManager : IIBLVMSocket
	{
		public int Status { get; set; } = (int)SocketStatus.Unconnected;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public IPacketFactory PacketFactory { get; private set; } = new PacketFactroy();

		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
		private NetworkStream networkStream;

		public void Conncet(IPEndPoint endPoint)
		{
			socket.Connect(endPoint);
			networkStream = new NetworkStream(socket);
		}

		public NetworkStream GetSocketStream() => networkStream;
	}
}
