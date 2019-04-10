using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Models
{
	public class BitLockerVolume
	{
		public string DeviceID { get; private set; }

		public string DriveLetter { get; private set; }

		public BitLockerVolume(string deviceID, string driveLetter)
		{
			DeviceID = deviceID;
			DriveLetter = driveLetter;
		}
	}
}
