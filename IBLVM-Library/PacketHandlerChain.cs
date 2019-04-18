using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using IBLVM_Util.Interfaces;

using IBLVM_Libaray.Interfaces;

namespace IBLVM_Util
{
	/// <summary>
	/// 책임 연쇄 패턴 (Chain of responsibility pattern) 구현을 위한 처리자 제어 클래스입니다.
	/// </summary>
	public class PacketHandlerChain
	{
		private List<IPacketHandler> handlers = new List<IPacketHandler>();
		private readonly IIBLVMSocket socket;

		/// <summary>
		/// IBLVMSocket 인스턴스로 PacketHandlerChain 클래스 인스턴스를 초기화합니다.
		/// </summary>
		/// <param name="socket">책임 연쇄에 사용될 IIBVLMSocket 인스턴스입니다.</param>
		public PacketHandlerChain(IIBLVMSocket socket) => this.socket = socket;

		/// <summary>
		/// IPacketHandler 인터페이스를 구현하는 처리자를 추가합니다.
		/// </summary>
		/// <param name="handler">추가할 IPacketHandler 처리자입니다.</param>
		public void AddHandler(IPacketHandler handler) => handlers.Add(handler);

		/// <summary>
		/// 패킷의 헤더를 파라매터로 책임 연쇄 패턴을 실행합니다.
		/// </summary>
		/// <param name="header">IPacketHandler 처리자들에게 전달될 IPacket 인스턴스입니다.</param>
		/// <returns>IPacketHandler 처리자들에 의하여 처리된 경우 true, 그렇지 않으면 false 입니다.</returns>
		public bool DoHandle(IPacket header)
		{
			bool isHandled = false;

			for (int i = 0; i < handlers.Count; i++)
				isHandled = handlers[i].Handle(header, socket) || isHandled ? true : false;

			return isHandled;
		}
	}
}
