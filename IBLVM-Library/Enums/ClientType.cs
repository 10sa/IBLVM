using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Enums
{
	/// <summary>
	/// 접속하는 클라이언트의 타입입니다.
	/// </summary>
	public enum ClientType : byte
	{
		/// <summary>
		/// 관리될 수 있는 디바이스입니다.
		/// </summary>
		Device,

		/// <summary>
		/// 디바이스들에 대한 관리 목적인 타입입니다.
		/// </summary>
		Manager
	}
}
