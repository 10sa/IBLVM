using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Packets;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using System.Net.Sockets;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// IBLVM 프로토콜 통신에 사용되는 소켓을 나타냅니다.
	/// </summary>
	public interface IIBLVMSocket
	{
		/// <summary>
		/// 상태 패턴 (State pattern)의 구현을 위한 소켓의 속성입니다.
		/// </summary>
		int Status { get; set; }

		/// <summary>
		/// 네트워크 통신에 사용되는 소켓의 NetworkStream 클래스 인스턴스를 가져옵니다.
		/// </summary>
		/// <returns>네트워크 통신에 사용되는 스트림입니다.</returns>
		NetworkStream GetSocketStream();

		/// <summary>
		/// 소켓에서 사용중인 CryptoProvider 클래스 인스턴스입니다..
		/// </summary>
		CryptoProvider CryptoProvider { get; set; }

		/// <summary>
		/// IBLVM 통신에서 패킷 생성에 사용되는 IPacketFactory 인스턴스를 반환합니다.
		/// </summary>
		IPacketFactory PacketFactory { get; }
	}
}
