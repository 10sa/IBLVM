using IBLVM_Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server.Models
{
	class ClientMessage
	{
		public PacketType Type { get; private set; }

		public object Payload { get; private set; }

		public ClientMessage(PacketType type, object payload)
		{
			Type = type;
			Payload = payload;
		}
	}
}
