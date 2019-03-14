using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBLVM_Libaray.BitLocker;

namespace IBLVM_Tests
{
	[TestClass]
	public class BitLockerTest
	{
		[TestMethod]
		public void GetVolumes()
		{
			foreach(var volume in BitLocker.GetVolumes())
			{
				Console.WriteLine(string.Format("DeviceID : {0}\nDrive Letter {1}\nProtection Status : {2}", volume.DeviceID, volume.DriveLetter, volume.GetProtectionStatus()));
			}
		}
	}
}
