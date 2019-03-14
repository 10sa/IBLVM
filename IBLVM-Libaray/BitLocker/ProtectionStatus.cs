using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.BitLocker
{
	enum ProtectionStatus : uint
	{
		/// <summary>
		/// 디스크가 잠겨 있음을 나타냅니다.
		/// </summary>
		Locked = 0,

		/// <summary>
		/// 디스크가 잠금 해제되어 있음을 나타냅니다.
		/// </summary>
		Unlocked,

		/// <summary>
		/// 디스크가 BitLocker으로 보호되고 있지 않음을 나타냅니다.
		/// </summary>
		NotBitlockerVolume
	}
}
