using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBLVM_Library;

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

		[TestMethod]
		public void Lock()
		{
			BitLocker volume = BitLocker.GetVolumes().Single((a) => { return a.DriveLetter == "E:"; });
			volume.Lock(true);
		}
	}
}
