using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Args
{
	class DrivesRequestEventArgs : EventArgs
	{
		public List<ClientDrive> Drives { get; private set; } = new List<ClientDrive>();

		public IDevice Device { get; private set; }

		public DrivesRequestEventArgs(IDevice device)
		{
			Device = device;
		}
	}
}
