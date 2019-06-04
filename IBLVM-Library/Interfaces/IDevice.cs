using System.Net;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 접속 디바이스에 대한 정보를 나타내는 인터페이스입니다.
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		/// 디바이스의 계정 정보입니다.
		/// </summary>
		IAccount Account { get; }

		/// <summary>
		/// 디바이스의 원격지 주소입니다.
		/// </summary>
		IPEndPoint DeviceIP { get; }

		/// <summary>
		/// 디바이스의 접속 타입입니다.
		/// </summary>
		ClientType Type { get; }
	}
}