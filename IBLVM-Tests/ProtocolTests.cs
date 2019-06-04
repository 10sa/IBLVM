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

			while (client.Status != (int)SocketStatus.Connected) ;
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
			while (client.Status != (int)SocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)SocketStatus.LoggedIn) ;
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
			client.Connect(new IPEndPoint(AccessIP, 47858));
			while (client.Status != (int)SocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)SocketStatus.LoggedIn) ;
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
			while (client.Status != (int)SocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)SocketStatus.LoggedIn) ;

			client.ExchangeIV();
			while (true) ;
		}
	}
}
