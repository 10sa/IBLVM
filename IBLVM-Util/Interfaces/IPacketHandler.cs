using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using IBLVM_Libaray.Interfaces;

namespace IBLVM_Util.Interfaces
{
	public interface IPacketHandler
	{
		bool Handle(IPacket header, IIBLVMSocket socket;
	}
}
