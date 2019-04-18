using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using IBLVM_Libaray.Interfaces;

namespace IBLVM_Util.Interfaces
{
	/// <summary>
	/// 책임 연쇄 패턴 (Chain of responsibility pattern) 구현을 위한 처리자 인터페이스입니다.
	/// </summary>
	public interface IPacketHandler
	{
		/// <summary>
		/// 주어진 파라매터를 통하여 수신된 패킷을 처리합니다.
		/// </summary>
		/// <param name="header">수신된 패킷의 헤더입니다.</param>
		/// <param name="socket">통신에 사용된 IIBLVMSocket 인스턴스입니다.</param>
		/// <returns>처리자가 처리 가능하고 처리한 경우 true, 그렇지 않으면 false 입니다.</returns>
		bool Handle(IPacket header, IIBLVMSocket socket);
	}
}
