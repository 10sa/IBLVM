using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Enums
{
	public enum PacketType : ushort
	{
		Hello = 0,
		ServerKeySend,
		ClientKeySend,
		ExchangeInitializeVector,
		BitLockerList
	}
}
