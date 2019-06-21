using IBLVM_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
	public class ClientDrive
	{
		public IPEndPoint IP { get; private set; }

		public DriveInformation Drive { get; private set; }

		public ClientDrive(IPEndPoint ip, DriveInformation drives)
		{
			IP = ip;
			Drive = drives;
		}

		public override string ToString() => $"{IP},{Drive}";

		public static ClientDrive FromString(string str)
		{
			int splitIdx = str.IndexOf(',');
			string[] ip = str.Substring(0, splitIdx).Split(':');
			return new ClientDrive(new IPEndPoint(IPAddress.Parse(ip[0]), int.Parse(ip[1])), DriveInformation.FromString(str.Substring(splitIdx + 1)));
		}
	}
}
