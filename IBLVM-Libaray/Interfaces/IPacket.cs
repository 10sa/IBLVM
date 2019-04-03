using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IBLVM_Libaray.Interfaces
{
	/// <summary>
	/// 패킷의 데이터 요구를 정의합니다.
	/// </summary>
	public interface IPacket
	{
		/// <summary>
		/// 패킷의 페이로드의 길이를 반환합니다.
		/// </summary>
		/// <returns>페이로드의 길이입니다.</returns>
		int GetPayloadSize();

		/// <summary>
		/// 패킷의 바이트화된 데이터를 반환합니다.
		/// </summary>
		/// <returns>패킷이 데이터화된 바이트입니다.</returns>
		byte[] GetPacketBytes();

		/// <summary>
		/// 패킷의 페이로드가 담긴 스트림을 반환합니다.
		/// </summary>
		/// <returns>패킷의 페이로드가 담긴 스트림입니다. 페이로드가 없는 경우 null 입니다.</returns>
		Stream GetPayloadStream();
	}
}
