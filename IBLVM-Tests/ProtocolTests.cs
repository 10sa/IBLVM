using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net;

using IBLVM_Server;
using IBLVM_Client;
using IBLVM_Client.Enums;
using IBLVM_Management;

namespace IBLVM_Tests
{
	[TestClass]
	public class ProtocolTests
	{
		private static readonly IPAddress AccessIP = IPAddress.Parse("192.168.10.160");

		[TestMethod]
		public void HandshakeTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 47857));
			server.Listen(5);

			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 47857));

			while (client.Status != (int)ClientSocketStatus.Connected) ;
            client.Dispose();
			server.Dispose();
        }

		[TestMethod]
		public void LoginTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 47858));
			server.Listen(5);

			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 47858));
			while (client.Status != (int)ClientSocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;
			client.Dispose();
			server.Dispose();
		}

		public void BitLockerListingTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 47859));
			server.Listen(5);
			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 47859));
			while (client.Status != (int)ClientSocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;

			IBLVMManager manager = new IBLVMManager();
			manager.Conncet(new IPEndPoint(IPAddress.Loopback, 47859));
			client.Dispose();
			server.Dispose();
		}

		[TestMethod]
		public void IVExchangeTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 47860));
			server.Listen(5);
			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 47860));
			while (client.Status != (int)ClientSocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;

			client.ExchangeIV();
			byte[] nextIV = client.CryptoProvider.NextIV;

			while (!nextIV.SequenceEqual(client.CryptoProvider.CryptoStream.IV)) ;
			client.Dispose();
			server.Dispose();
		}
	}
}
