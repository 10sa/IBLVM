using System.Net;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
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
	}
}