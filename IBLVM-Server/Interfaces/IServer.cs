using IBLVM_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Interfaces
{
	internal interface IServer
	{
		ISession Session { get; }

		IPacketFactory PacketFactory { get; }

		IDeviceController DeviceController { get; }
	}
}
