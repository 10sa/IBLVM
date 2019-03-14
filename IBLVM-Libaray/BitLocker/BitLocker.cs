using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.ComponentModel;

namespace IBLVM_Libaray.BitLocker
{
	class BitLocker
	{
		private ManagementObject bitlockerObject;

		private BitLocker() { }
	
		private BitLocker(ManagementObject bitlockerObject)
		{
			this.bitlockerObject = bitlockerObject;
		}

		#region Private methods
		private ManagementBaseObject InvokeMethod(string method, params object[] args)
		{
			ManagementBaseObject result = (ManagementBaseObject)bitlockerObject.InvokeMethod(method, args);
			ErrorValidation(result);

			return result;
		}

		private void ErrorValidation(ManagementBaseObject result)
		{
			uint code = (uint)result["returnValue"];
			if (code == 0x0)
				throw new Win32Exception(Convert.ToInt32(code));
		}

		#endregion

		#region Public methods
		/// <summary>
		/// BitLocker가 적용된 볼륨을 반환합니다.
		/// </summary>
		/// <returns>BitLocker가 적용된 볼륨 목록.</returns>
		public static BitLocker[] GetVolumes()
		{
			var path = new ManagementPath(@"Root\CIMV2\Security\MicrosoftVolumeEncryption") {
				ClassName = "Win32_EncryptableVolume"
			};

			List<BitLocker> collection = new List<BitLocker>();
			foreach (ManagementObject volume in new ManagementClass(new ManagementScope(path), path, new ObjectGetOptions()).GetInstances())
				collection.Add(new BitLocker(volume));

			return collection.ToArray();
		}

		public string DeviceID => (string)bitlockerObject["DeviceID"];

		public string DriveLetter => (string)bitlockerObject["DriveLetter"];
		
		public ProtectionStatus GetProtectionStatus()
		{
			ManagementBaseObject result = InvokeMethod("GetProtectionStatus");

			return (ProtectionStatus)result["ProtectionStatus"];
		}
		#endregion
	}
}
