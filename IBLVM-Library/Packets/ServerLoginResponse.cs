﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;

using IBLVM_Library.Enums;

namespace IBLVM_Library.Packets
{
	public sealed class ServerLoginResponse : BaseActionResultPacket
	{
		public ServerLoginResponse(bool isSuccess) : base(isSuccess, PacketType.ServerLoginResponse) { }
	}
}
