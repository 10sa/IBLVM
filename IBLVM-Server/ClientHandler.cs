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
using IBLVM_Libaray.Models;

using IBLVM_Server.Enums;

namespace IBLVM_Server
{
	/// <summary>
	/// 클라이언트 소켓에 대한 응답 처리를 위한 클래스입니다.
	/// </summary>
	class ClientHandler : IIBLVMSocket, IDisposable
	{
		public Thread Thread { get; private set; }

		private readonly NetworkStream socketStream;
		private readonly PacketHandlerChain chain;
		private readonly IPacketFactory factory;
		private byte[] buffer;
		private readonly Socket socket;

		public ClientHandler(Socket socket, IPacketFactory factory)
		{
			this.socket = socket;
			this.factory = factory;

			buffer = new byte[factory.PacketSize * 2];
			socketStream = new NetworkStream(socket);
			chain = new PacketHandlerChain(this);
		}

		public void Start()
		{
			Thread = new Thread(() =>
			{
				while(true)
				{
					try
					{
						SocketUtil.ReceiveFull(socketStream, buffer, factory.PacketSize);
						IPacket header = factory.ParseHeader(buffer);
						chain.DoHandle(header);
					}
					finally {
						Dispose();
					}
				}
			});
		}

		#region IIBLVMSocket Implements
		public int Status { get; set; } = (int)SocketStatus.Uninitialized;

		public CryptoProvider CryptoProvider { get; set; }

		public NetworkStream GetSocketStream() => socketStream;
		#endregion

		#region IDisposable Implements
		public void Dispose()
		{
			socket.Disconnect(false);
			socket.Dispose();

			socketStream.Close();
			CryptoProvider.Dispose();
			buffer = null;
		}
		#endregion
	}
}
