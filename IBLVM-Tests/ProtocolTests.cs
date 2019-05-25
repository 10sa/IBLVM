using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net;

using IBLVM_Server;
using IBLVM_Client;

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

			while (client.Status != (int)IBLVM_Client.Enums.SocketStatus.Connected) ;
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
			while (client.Status != (int)IBLVM_Client.Enums.SocketStatus.Connected) ;

			client.Login("Test", "Test");
			while (client.Status != (int)IBLVM_Client.Enums.SocketStatus.LoggedIn) ;
			client.Dispose();
			server.Dispose();
		}
	}
}
