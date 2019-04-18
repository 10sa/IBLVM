using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using IBLVM_Util;
using IBLVM_Util.Interfaces;

using IBLVM_Library;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using IBLVM_Server.Enums;

namespace IBLVM_Server
{
	/// <summary>
	/// 클라이언트 소켓에 대한 응답 처리를 위한 클래스입니다.
	/// </summary>
	class ClientHandler : IIBLVMSocket, IDisposable
	{
		public Thread Thread { get; private set; }

		public IPacketFactory PacketFactory { get; private set; }


		private readonly NetworkStream socketStream;
		private readonly ServerHandlerChain chain;
		private byte[] buffer;
		private readonly Socket socket;

		public ClientHandler(Socket socket, IPacketFactory packetFactory)
		{
			this.socket = socket;
			this.PacketFactory = packetFactory;

			buffer = new byte[packetFactory.PacketSize * 2];
			socketStream = new NetworkStream(socket);
			chain = new ServerHandlerChain(this, packetFactory);
		}

		public void Start()
		{
			Thread = new Thread(() =>
			{
				while(true)
				{
					try
					{
						Utils.ReadFull(socketStream, buffer, PacketFactory.PacketSize);
						IPacket header = PacketFactory.ParseHeader(buffer);
						chain.DoHandle(header);
					}
					catch(Exception) {
						Dispose();
					}
				}
			});

			Thread.Start();
		}

		#region IIBLVMSocket Implements
		public int Status { get; set; } = (int)SocketStatus.Uninitialized;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

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
