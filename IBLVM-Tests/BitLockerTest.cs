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
			BitLocker.GetVolumes();
		}
	}
}
