using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IBLVM_Libaray.Enums;

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

		/// <summary>
		/// 인스턴스의 파싱 방법을 이용하여 데이터를 파싱합니다.
		/// </summary>
		/// <param name="payloadSize">파싱할 데이터의 크기입니다.</param>
		/// <param name="stream">파싱할 데이터를 읽어들일 스트림입니다.</param>
		void ParsePayload(int payloadSize, Stream stream);

		/// <summary>
		/// 패킷의 역활을 나타내는 타입입니다.
		/// </summary>
		PacketType Type { get; }

		/// <summary>
		/// 패킷에 사용되는 매직 바이트를 반환합니다.
		/// </summary>
		byte[] MagicBytes { get; }
	}
}
