using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Enums
{
	public enum PacketType : short
	{
		Hello = 0,
		ServerKeyResponse,
		ClientKeyResponse,
		ExchangeInitializeVector,
		ClientLoginRequest,
		ServerLoginResponse,
		BitLockerList
	}
}
