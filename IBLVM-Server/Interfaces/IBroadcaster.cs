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

		event Action<BitLockerControlEventArgs> BroadcastBitLockerControl;

		ClientDrive[] RequestDrives(IDevice device);

		bool RequestBitLockerLock(IDevice device, DriveInformation drive);

		bool RequestBitLockerUnlock(IDevice device, DriveInformation drive, string password);
	}
}
