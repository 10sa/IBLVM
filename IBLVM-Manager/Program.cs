using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

			Console.WriteLine("Please enter server address.");
			manager.BaseManager.Conncet(new IPEndPoint(IPAddress.Parse(Console.ReadLine()), 54541));
			while (manager.BaseManager.Status != (int)ClientSocketStatus.Connected) ;

			Console.WriteLine("Connected!");
			manager.Login();

			while (true)
			{
				Console.WriteLine("1. Device list");
				Console.WriteLine("2. Drive list");

				string input = Console.ReadLine();
				lock (manager.Lock)
				{
					if (input[0] == '1')
						manager.BaseManager.GetDeviceList();
					else if (input[0] == '2')
					{
						string[] arguments = input.Split(' ');
						manager.BaseManager.GetDeviceDrives(manager.Devices[int.Parse(arguments[1])]);
					}

					Monitor.Wait(manager.Lock);
				}
			}
		}
		private class Manager
		{
			public IBLVMManager BaseManager { get; private set; }

			public object Lock { get; private set; } = new object();

			private bool ignoreDisplay = false;

			public IDevice[] Devices { get; private set; }

			public Manager(IBLVMManager manager)
			{
				BaseManager = manager;
				manager.OnDevicesReceived += Manager_OnDevicesReceived;
				manager.OnDrivesReceived += Manager_OnDrivesReceived;
				manager.OnBitLockerCommandResponseReceived += Manager_OnBitLockerCommandResponseReceived;
			}

			private void Manager_OnBitLockerCommandResponseReceived(bool obj)
			{
				lock (Lock)
				{
					if (obj)
						Console.WriteLine("Successfully executed!");
					else
						Console.WriteLine("Error! execute failure!");

					Monitor.PulseAll(Lock);
				}
			}

			private void Manager_OnDrivesReceived(IBLVM_Library.Models.ClientDrive[] obj)
			{
				lock (Lock)
				{
					Console.WriteLine("Drives received!");

					int cnt = 0;
					foreach (var drive in obj)
					{
						Console.WriteLine(string.Format("{0}. {1}", cnt, string.Join("\t", drive.Drive.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))));
						cnt++;
					}

					Monitor.PulseAll(Lock);
				}
			}

			private void Manager_OnDevicesReceived(IBLVM_Library.Interfaces.IDevice[] obj)
			{
				lock (Lock)
				{
					if (!ignoreDisplay)
					{
						Console.WriteLine("Device list received.");
						Console.WriteLine("-----------------------------");
						int cnt = 0;
						foreach (var device in obj)
						{
							Console.WriteLine(string.Format("{0}.\t{1}", cnt, device.DeviceIP));
							cnt++;
						}

						Console.WriteLine("-----------END-----------");
					}
					else
						ignoreDisplay = false;

					Devices = obj;
					Monitor.PulseAll(Lock);
				}
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
