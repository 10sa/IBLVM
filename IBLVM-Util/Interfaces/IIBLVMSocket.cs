using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Models;
using IBLVM_Libaray.Interfaces;

using System.Net.Sockets;

namespace IBLVM_Util.Interfaces
{
	public interface IIBLVMSocket
	{
		int Status { get; set; }

		NetworkStream GetSocketStream();

		CryptoProvider CryptoProvider { get; set; }

		IPacketFactory PacketFactory { get; }
	}
}
