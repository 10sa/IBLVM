using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using IBLVM_Util;
using IBLVM_Util.Interfaces;

using IBLVM_Libaray.Interfaces;

using IBLVM_Server.Enums;

using CryptoStream;

namespace IBLVM_Server
{
	/// <summary>
	/// 클라이언트 소켓에 대한 응답 처리를 위한 클래스입니다.
	/// </summary>
	class ClientHandler : IIBLVMSocket
	{
		public Thread Thread { get; private set; }

		public SocketStatus Status { get; private set; }

		private CryptoMemoryStream cryptoStream;
		private NetworkStream socketStream;
		private byte[] buffer = new byte[256];
		private PacketHandlerChain chain;
		private IPacketFactory factory;
		private Socket socket;

		public ClientHandler(Socket socket, IPacketFactory factory)
		{
			this.socket = socket;
			this.factory = factory;
			socketStream = new NetworkStream(socket);

			chain = new PacketHandlerChain(this);
		}

		public void Start()
		{
			Thread = new Thread(() =>
			{
				SocketUtil.ReceiveFull(socketStream, buffer, factory.PacketSize);
				IPacket header = factory.ParseHeader(buffer);
				chain.DoHandle(header);
			});
		}

		public void SetSocketStatus(int status) => Status = (SocketStatus)status;

		public NetworkStream GetSocketStream() => socketStream;

		public CryptoMemoryStream GetCryptoStream() => cryptoStream;
	}
}
