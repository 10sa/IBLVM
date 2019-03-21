using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	sealed class HelloResponsePacket : BasePacket
	{
		public HelloResponsePacket() : base(Enums.PacketType.Ack) { }

		public override Stream GetPayloadStream() => null;
	}
}
