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
	public class PacketHandlerChain
	{
		private List<IPacketHandler> handlers = new List<IPacketHandler>();
		private readonly IIBLVMSocket socket;

		public PacketHandlerChain(IIBLVMSocket socket) => this.socket = socket;

		public void AddHandler(IPacketHandler handler) => handlers.Add(handler);

		public bool DoHandle(IPacket header)
		{
			bool isHandled = false;

			for (int i = 0; i < handlers.Count; i++)
				isHandled = handlers[i].Handle(header, socket) || isHandled ? true : false;

			return isHandled;
		}
	}
}
