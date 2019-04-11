using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Enums
{
	enum SocketStatus : int
	{
		/// <summary>
		/// IBLVM 프로토콜의 핸드셰이크 과정이 이뤄지지 않았음을 의미합니다.
		/// </summary>
		Uninitialized,

		/// <summary>
		/// 클라이언트에게 키를 전송하였으며, 클라이언트 측의 공개 키를 기다리는 상태입니다.
		/// </summary>
		ServerKeyResponsed,

		/// <summary>
		/// 클라이언트-서버간 IBLVM 프로토콜의 핸드셰이크를 완료하였으며 통신이 가능함을 의미합니다.
		/// </summary>
		Connected
	}
}
