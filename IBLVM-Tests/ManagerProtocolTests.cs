using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Server;
using IBLVM_Client;
using IBLVM_Management;

namespace IBLVM_Tests
{
	[TestClass]
	public class ManagerProtocolTests
	{
		private static readonly IPAddress AccessIP = IPAddress.Parse("192.168.10.160");

		[TestMethod]
		public void HandshakeTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 40001));
			server.Listen(5);
			server.Start();

			IBLVMManager manager = new IBLVMManager();
			manager.Conncet(new IPEndPoint(AccessIP, 40001));

			while (manager.Status != (int)ClientSocketStatus.Connected) ;
			server.Dispose();
			manager.Dispose();
		}
	}
}
