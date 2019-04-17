using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Interfaces
{
	public interface IPacketFactory
	{
		IPacket CreateClientHello();

		ICryptoExchanger CreateServerKeyResponse(byte[] data);

		ICryptoExchanger CreateClientKeyResponse(byte[] data);

		IPacket ParseHeader(byte[] data);

		byte[] MagicBytes { get; }

		int PacketSize { get; }
	}
}
