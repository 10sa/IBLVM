using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Enums
{
	public enum ClientSocketStatus : int
	{
		Disconnected,
		Handshaking,
		Connected,
		LoggedIn
	}
}
