using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using IBLVM_Library;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using IBLVM_Server.Interfaces;
using IBLVM_Server.Enums;
using System.Net;
using IBLVM_Server.Args;
using IBLVM_Server.Models;
using IBLVM_Library.Enums;
using System.IO;

namespace IBLVM_Server
{
	/// <summary>
	/// 클라이언트 소켓에 대한 응답 처리를 위한 클래스입니다.
	/// </summary>
	class ClientHandler : IIBLVMSocket, IDisposable
	{
		#region IIBLVMSocket Implements
		public int Status { get; set; } = (int)SocketStatus.Uninitialized;

		public CryptoProvider CryptoProvider { get; set; } = new CryptoProvider();

		public IPacketFactory PacketFactory { get; private set; }

		public NetworkStream SocketStream { get; private set; }
		#endregion

		public Thread Thread { get; private set; }

		public event Action<ClientHandler> OnHandlerDisposed = (a) => { };

		private MessageQueue messageQueue = new MessageQueue();
		private readonly ServerHandlerChain chain;
		private readonly IBroadcaster broadcaster;
		private readonly Socket socket;
		private readonly IServer server;
		private IDevice device;
		private byte[] buffer;

		public ClientHandler(Socket socket, IServer server, IBroadcaster broadcaster)
		{
			this.broadcaster = broadcaster;
			this.socket = socket;
			this.server = server;

			buffer = new byte[server.PacketFactory.PacketSize * 2];
			SocketStream = new NetworkStream(socket);
			chain = new ServerHandlerChain(this, messageQueue, server, broadcaster);
			PacketFactory = server.PacketFactory;

			broadcaster.BroadcastDrivesRequest += Broadcaster_BroadcastDrivesRequest;
			broadcaster.BroadcastBitLockerControl += Broadcaster_BroadcastBitLockerControl;
			chain.OnClientLoggedIn += OnClientLoggedIn;
		}

		public void Start()
		{
            Thread = new Thread(() =>
            {
				try
				{
					while (true)
					{
						Utils.ReadFull(SocketStream, buffer, PacketFactory.PacketSize);
						IPacket header = PacketFactory.ParseHeader(buffer);
						chain.DoHandle(header);
					}
				}
				catch (ThreadAbortException) { }
				catch (ObjectDisposedException) { }
				catch (SocketException)
				{
					if (socket.Connected)
						throw;
				}
				catch (IOException e)
				{
					if (e.InnerException.GetType() != typeof(SocketException) && !socket.Connected)
						return;
				}
				finally
				{
					Dispose();
				}
				
            })
            {
                Name = string.Format("IBLVM Client handler [{0}]", socket.RemoteEndPoint.ToString())
            };
            Thread.Start();
		}

		private void Broadcaster_BroadcastDrivesRequest(DrivesRequestEventArgs args)
		{
			if (Status == (int)SocketStatus.LoggedIn && device.Type == ClientType.Device && args.Device.Account.Id == device.Account.Id)
			{
				IPacket packet = PacketFactory.CreateServerDrivesRequest();
				Utils.SendPacket(SocketStream, packet);

				messageQueue.Wait(PacketType.ClientDrivesResponse);
				ClientMessage message = messageQueue.Dequeue();
				foreach (var drive in (DriveInformation[])message.Payload)
					args.Drives.Add(new ClientDrive((IPEndPoint)socket.RemoteEndPoint, drive));
			}
		}

		private void Broadcaster_BroadcastBitLockerControl(BitLockerControlEventArgs args)
		{
			if (Status == (int)SocketStatus.LoggedIn && device.Type == ClientType.Device && args.Device.Account.Id == device.Account.Id)
			{
				IPacket packet = PacketFactory.createserver
			}
		}

		#region Handler event hooks
		private void OnClientLoggedIn(IAuthInfo authInfo)
		{
			device = new Device((IPEndPoint)socket.RemoteEndPoint, authInfo);

			if (authInfo.Type == ClientType.Device)
				server.DeviceController.AddDevice(authInfo.Account.Id, device);
		}
		#endregion

		#region IDisposable Implements
		public void Dispose()
		{
			if (socket.Connected)
				socket.Disconnect(false);

			socket.Dispose();
			SocketStream.Close();
			CryptoProvider.Dispose();
			buffer = null;

			OnHandlerDisposed(this);
		}
		#endregion
	}
}
