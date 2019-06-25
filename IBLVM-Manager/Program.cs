using IBLVM_Library.Enums;
using IBLVM_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBLVM_Manager
{
	class Program
	{
		static void Main(string[] args)
		{
			Manager manager = new Manager(new IBLVMManager());
			manager.Login();

			while (true)
			{
				Console.WriteLine("1. Device list");
				Console.WriteLine("2. Drive list");

				string input = Console.ReadLine();
				lock (manager.Lock)
				{
					if (input.Trim() == "1")
						manager.BaseManager.GetDeviceList();
					else if (input.Trim() == "2")
						manager.BaseManager.GetDeviceList();

					Monitor.Wait(manager.Lock);
				}
			}
		}
		private class Manager
		{
			public IBLVMManager BaseManager { get; private set; }
			public object Lock { get; private set } = new object();

			public Manager(IBLVMManager manager)
			{
				BaseManager = manager;
				manager.OnDevicesReceived += Manager_OnDevicesReceived;
				manager.OnDrivesReceived += Manager_OnDrivesReceived;
				manager.OnBitLockerCommandResponseReceived += Manager_OnBitLockerCommandResponseReceived;
			}

			private void Manager_OnBitLockerCommandResponseReceived(bool obj)
			{
				if (obj)
					Console.WriteLine("Successfully executed!");
				else
					Console.WriteLine("Error! execute failure!");

				Monitor.PulseAll(Lock);
		}

			private void Manager_OnDrivesReceived(IBLVM_Library.Models.ClientDrive[] obj)
			{
				Console.WriteLine("Drives received!");
				foreach (var drive in obj)
					string.Join("\t", drive.Drive.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

				Monitor.PulseAll(Lock);
			}

			private void Manager_OnDevicesReceived(IBLVM_Library.Interfaces.IDevice[] obj)
			{
				Console.WriteLine("Device list received.");
				foreach (var device in obj)
					Console.WriteLine(device.DeviceIP);

				Monitor.PulseAll(Lock);
			}

			public void Login()
			{
				Console.WriteLine("Connected!");
				Console.WriteLine("Please login to server.");
				Console.Write("ID :");
				string id = Console.ReadLine();
				Console.Write("Password : ");
				string password = Console.ReadLine();


				BaseManager.Login(id, password);
				while (BaseManager.Status != (int)ClientSocketStatus.LoggedIn) ;
			}
		}
	}
}
