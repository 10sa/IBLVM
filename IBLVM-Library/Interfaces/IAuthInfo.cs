using IBLVM_Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 인증 정보를 나타내는 인터페이스입니다.
	/// </summary>
	public interface IAuthInfo
	{
		/// <summary>
		/// 접속할 계정 정보입니다.
		/// </summary>
		IAccount Account { get; }

		/// <summary>
		/// 디바이스가 어떤 형태로의 접속인지를 나타냅니다.
		/// </summary>
		ClientType Type { get; }
	}
}
