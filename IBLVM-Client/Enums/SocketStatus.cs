using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Client.Enums
{
	public enum SocketStatus : int
	{
		Disconnected,
		Handshaking,
		Connected,
		LoggedIn
	}
}
