using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Management.Enums
{
	public enum SocketStatus : int 
	{
		Unconnected,
		Handshaking,
		Connected,
		Loggedin
	}
}
