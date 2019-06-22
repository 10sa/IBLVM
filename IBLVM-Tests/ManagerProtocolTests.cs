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
		public void ConnectionTest()
		{
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 40001));
			server.Listen(5);
			server.Start();

			IBLVMManager manager = new IBLVMManager();
			manager.Conncet(new IPEndPoint(AccessIP, 40001));

			while (manager.Status != (int)ClientSocketStatus.Connected) ;
			manager.Login("1234", "1234");

			while (manager.Status != (int)ClientSocketStatus.LoggedIn) ;
			server.Dispose();
			manager.Dispose();
		}

		[TestMethod]
		public void DeviceListingTest()
		{
			bool isEndable = false;
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 40001));
			server.Listen(5);
			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 40001));
			while (client.Status != (int)ClientSocketStatus.Connected) ;

			client.Login("1234", "1234");
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;


			IBLVMManager manager = new IBLVMManager();
			manager.Conncet(new IPEndPoint(AccessIP, 40001));

			while (manager.Status != (int)ClientSocketStatus.Connected) ;
			manager.Login("1234", "1234");

			while (manager.Status != (int)ClientSocketStatus.LoggedIn) ;
			manager.OnDevicesReceived += (a) =>
			{
				foreach (var device in a)
					Console.WriteLine(device.ToString());

				isEndable = true;
			};

			manager.GetDeviceList();

			while (manager.Status != (int)ClientSocketStatus.LoggedIn || !isEndable) ;
			server.Dispose();
			manager.Dispose();
		}

		[TestMethod]
		public void DriveListingTest()
		{
			bool isEndable = false;
			IBLVMServer server = new IBLVMServer(new SessionControl());
			server.Bind(new IPEndPoint(IPAddress.Any, 40001));
			server.Listen(5);
			server.Start();

			IBLVMClient client = new IBLVMClient();
			client.Connect(new IPEndPoint(AccessIP, 40001));
			while (client.Status != (int)ClientSocketStatus.Connected) ;

			client.Login("1234", "1234");
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;


			IBLVMManager manager = new IBLVMManager();
			manager.Conncet(new IPEndPoint(AccessIP, 40001));

			while (manager.Status != (int)ClientSocketStatus.Connected) ;
			manager.Login("1234", "1234");

			while (manager.Status != (int)ClientSocketStatus.LoggedIn) ;
			manager.OnDevicesReceived += (devices) =>
			{
				manager.OnDrivesReceived += (drives) =>
				{
					foreach (var device in drives)
						Console.WriteLine(device);

					isEndable = true;
				};

				manager.GetDeviceDrives(devices[0]);
			};

			manager.GetDeviceList();
			while (manager.Status != (int)ClientSocketStatus.LoggedIn || !isEndable) ;
			server.Dispose();
			// manager.Dispose();
			// client.Dispose();
		}
	}
}
