using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
{
	public class Device : IDevice
	{
		public IPAddress DeviceIP { get; private set; }

		public IAccount Account { get; private set; }

		public Device(IPAddress deviceIP, IAccount account)
		{
			DeviceIP = deviceIP;
			Account = account;
		}

		public override string ToString() => DeviceIP.ToString();

		public static Device FromString(string str)
		{
			string[] data = str.Split(',');
			return new Device(IPAddress.Parse(data[0]), new Account(data[0], data[1]));
		}
	}
}
