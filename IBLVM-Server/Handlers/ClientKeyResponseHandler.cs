using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using System.Security.Cryptography;

using IBLVM_Libaray.Interfaces;
using IBLVM_Util.Interfaces;
using IBLVM_Libaray.Models;
using IBLVM_Util;

using IBLVM_Server.Enums;

using IBLVM_Libaray.Enums;

namespace IBLVM_Server.Handlers
{
	class ClientKeyResponseHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ClientKeyResponse)
			{
				if (socket.Status != (int)SocketStatus.ServerKeyResponsed)
					throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

				CryptoProvider provider = socket.CryptoProvider;

				byte[] publicKey = SocketUtil.ReceiveFull(socket.GetSocketStream(), header.GetPayloadSize());
				provider.SharedKey = provider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

				return true;
			}

			return false;
		}
	}
}
