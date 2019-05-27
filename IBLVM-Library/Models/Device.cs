using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

namespace IBLVM_Library.Models
{
	public class Device : IDevice
	{
		public IPEndPoint DeviceIP { get; private set; }

		public IAccount Account { get; private set; }

		public ClientType Type { get; private set; }

		public Device(IPEndPoint deviceIP, IAccount account, ClientType type)
		{
			DeviceIP = deviceIP;
			Account = account;
			Type = type;
		}

		public override string ToString() => DeviceIP.ToString() + "," + Account.ToString() + "," + Type.ToString();

		public static Device FromString(string str)
		{
			string[] data = str.Split(',');
			string[] ip = data[0].Split(':');
			return new Device(new IPEndPoint(IPAddress.Parse(ip[0]), int.Parse(ip[1])), new Account(data[0], data[1]), (ClientType)byte.Parse(data[2]));
		}
	}
}
