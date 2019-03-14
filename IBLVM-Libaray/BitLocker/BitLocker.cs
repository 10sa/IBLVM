using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// Allow internal access to UnitTest project.
[assembly: InternalsVisibleTo("IBLVM-Tests")]

namespace IBLVM_Libaray.BitLocker
{
	internal class BitLocker
	{
		private ManagementObject bitlockerObject;

		private BitLocker() { }
	
		private BitLocker(ManagementObject bitlockerObject)
		{
			this.bitlockerObject = bitlockerObject;
		}

		#region Private methods
		private object InvokeMethod(string method, params object[] args)
		{
			object result = bitlockerObject.InvokeMethod(method, args);
			if (result is uint)
				ErrorValidation((uint)result);
			else
				ErrorValidation((ManagementBaseObject)result);

			return result;
		}

		private ManagementBaseObject InvokeMethod(string method)
		{
			ManagementBaseObject result = bitlockerObject.InvokeMethod(method, null, null);
			ErrorValidation(result);

			return result;
		}

		private void ErrorValidation(ManagementBaseObject result)
		{
			uint code = (uint)result["returnValue"];
			if (code != 0x0)
				throw new Win32Exception(Convert.ToInt32(code));
		}

		private void ErrorValidation(uint result)
		{
			if (result != 0x0)
				throw new Win32Exception(unchecked((int)result));
		}

		#endregion

		#region Public methods
		/// <summary>
		/// 모든 볼륨을 가져옵니다.
		/// </summary>
		/// <returns>볼륨들입니다.</returns>
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

		/// <summary>
		/// 디바이스 ID 입니다.
		/// </summary>
		public string DeviceID => (string)bitlockerObject["DeviceID"];

		/// <summary>
		/// 시스템 상에서 표기되는 드라이브 문자입니다.
		/// </summary>
		public string DriveLetter => (string)bitlockerObject["DriveLetter"];
		
		/// <summary>
		/// 볼륨의 보호 상태를 가져옵니다.
		/// </summary>
		/// <returns>볼륨의 보호 상태입니다.</returns>
		public ProtectionStatus GetProtectionStatus()
		{
			ManagementBaseObject result = InvokeMethod("GetProtectionStatus");

			Console.WriteLine(result["ProtectionStatus"]);
			return (ProtectionStatus)result["ProtectionStatus"];
		}

		/// <summary>
		/// BitLocker 볼륨을 분리하고 시스템 메모리상에서 복호화 키를 제거합니다.
		/// </summary>
		public void Lock()
		{
			Lock(false);
		}

		/// <summary>
		/// BitLocker 볼륨을 분리하고 시스템 메모리상에서 복호화 키를 제거합니다.
		/// </summary>
		/// <param name="forceLock">true인 경우 해당 디스크를 강제로 분리합니다.</param>
		public void Lock(bool forceLock)
		{
			InvokeMethod("Lock", forceLock);
		}
		#endregion
	}
}
