using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
	public sealed class BitLockerVolume
	{
		public string DeviceID { get; private set; }

		public string DriveLetter { get; private set; }

		public BitLockerVolume(string deviceID, string driveLetter)
		{
			DeviceID = deviceID;
			DriveLetter = driveLetter;
		}

		public override string ToString() => DeviceID + ',' + DriveLetter;

		public static BitLockerVolume FromString(string str)
		{
			string[] data = str.Split(';');
			return new BitLockerVolume(data[0], data[1]);
		}
	}
}
