using IBLVM_Client;
using IBLVM_Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Device
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				try
				{
					IBLVMClient client = new IBLVMClient();
					Console.WriteLine("Please enter server address.");
					client.Connect(new IPEndPoint(IPAddress.Parse(Console.ReadLine()), 54541));
					while (client.Status != (int)ClientSocketStatus.Connected) ;

					Login(client);
					Console.WriteLine("Successfully logged in!");
					client.Receiver.Join();
				}
				catch (Exception)
				{
					continue;
				}
			}
		}

		private static void Login(IBLVMClient client)
		{
			Console.WriteLine("Connected!");
			Console.WriteLine("Please login to server.");
			Console.Write("ID :");
			string id = Console.ReadLine();
			Console.Write("Password : ");
			string password = Console.ReadLine();


			client.Login(id, password);
			while (client.Status != (int)ClientSocketStatus.LoggedIn) ;
		}
	}
}
