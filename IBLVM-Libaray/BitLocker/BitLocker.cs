using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace IBLVM_Libaray.BitLocker
{
	class BitLocker
	{
		private ManagementBaseObject bitlockerObject;

		private BitLocker() { }
	
		private BitLocker(ManagementBaseObject bitlockerObject)
		{
			this.bitlockerObject = bitlockerObject;
		}

		public static BitLocker[] GetVolumes()
		{
			var path = new ManagementPath(@"Root\CIMV2\Security\MicrosoftVolumeEncryption") {
				ClassName = "Win32_EncryptableVolume"
			};

			List<BitLocker> collection = new List<BitLocker>();
			foreach (var volume in new ManagementClass(new ManagementScope(path), path, new ObjectGetOptions()).GetInstances())
				collection.Add(new BitLocker(volume));

			return collection.ToArray();
		}
	}
}
