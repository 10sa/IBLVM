using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using IBLVM_Server.Args;
using IBLVM_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Interfaces
{
	interface IBroadcaster
	{
		event Action<DrivesRequestEventArgs> BroadcastDrivesRequest;

		ClientDrive[] RequestDrives(IDevice device);
	}
}
