using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 식별자-비밀번호 기반의 사용자 인증 정보를 제공합니다.
	/// </summary>
	public interface IAuthentication : IPacket
	{
		/// <summary>
		/// 사용자의 식별자입니다.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// 사용자의 비밀번호입니다.
		/// </summary>
		string Password { get; }
	}
}
