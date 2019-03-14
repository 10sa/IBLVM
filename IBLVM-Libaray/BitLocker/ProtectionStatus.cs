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
		/// 디스크가 BitLocker에 의하여 보호되지 않음을 나타냅니다.
		/// </summary>
		Unprotected = 0,

		/// <summary>
		/// 디스크가 BitLocker에 의하여 보호되고 있음을 나타냅니다.
		/// </summary>
		Protected,

		/// <summary>
		/// 디스크의 잠금 여부를 알수 없는 경우를 나타냅니다. 볼륨이 잠겨 있는 경우, 이 상태를 나타낼 수 있습니다.
		/// </summary>
		Unknown
	}
}
