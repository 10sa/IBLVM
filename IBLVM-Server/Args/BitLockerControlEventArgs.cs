using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Args
{
	class BitLockerControlEventArgs
	{
		public IDevice Device { get; private set; }

		public DriveInformation Drive { get; private set; }

		public bool Lock { get; private set; }

		public string Password { get; private set; }

		public bool IsSuccess { get; set; } = false;

		public BitLockerControlEventArgs(IDevice device, DriveInformation drive, bool @lock)
		{
			Device = device;
			Drive = drive;
			Lock = @lock;
		}

		public BitLockerControlEventArgs(IDevice device, DriveInformation drive, bool @lock, string password) : this(device, drive, @lock)
		{
			Password = password;
		}
	}
}
