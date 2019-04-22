using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

using System.IO;

namespace IBLVM_Library.Models
{
	class ServerBitLockersRequest : BasePacket
	{
		public ServerBitLockersRequest() : base(PacketType.ServerBitLockersRequest) { }
	}
}
