using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Interfaces
{
	/// <summary>
	/// 패킷 생성에 사용되는 팩토리 클래스 인터페이스입니다..
	/// </summary>
	public interface IPacketFactory
	{
		/// <summary>
		/// ClientHello 패킷을 생성합니다.
		/// </summary>
		/// <returns>생성된 ClientHello 패킷입니다.</returns>
		IPacket CreateClientHello();

		/// <summary>
		/// 암호화 키 교환에 사용되는 ServerKeyResponse 패킷을 생성합니다.
		/// </summary>
		/// <param name="data">키 교환 과정에서 사용될 데이터입니다.</param>
		/// <returns>생성된 ServerKeyResponse 패킷입니다.</returns>
		ICryptoExchanger CreateServerKeyResponse(byte[] data);

		/// <summary>
		/// 암호화 키 교환에 사용되는 ClientKeyResponse 패킷을 생성합니다.
		/// </summary>
		/// <param name="data">키 교환 과정에서 사용될 데이터입니다.</param>
		/// <returns>생성된 ClientKeyResponse 패킷입니다.</returns>
		ICryptoExchanger CreateClientKeyResponse(byte[] data);

		/// <summary>
		/// 바이트 배열에서 패킷의 헤더 부분만 파싱합니다.
		/// </summary>
		/// <param name="data">파싱할 데이터가 든 바이트 배열입니다.</param>
		/// <returns>헤더 부분의 데이터만을 가진 인터페이스를 구현하는 클래스 인스턴스입니다.</returns>
		IPacket ParseHeader(byte[] data);

		/// <summary>
		/// 패킷에 사용되는 매직 바이트입니다.
		/// </summary>
		byte[] MagicBytes { get; }

		/// <summary>
		/// 패킷 헤더 부분의 크기입니다.
		/// </summary>
		int PacketSize { get; }
	}
}
