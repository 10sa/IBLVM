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

using SecureStream;

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
				ICryptoExchanger packet = socket.PacketFactory.CreateClientKeyResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				provider.SharedKey = provider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(packet.Data, CngKeyBlobFormat.EccPublicBlob));
				provider.CryptoStream = new CryptoMemoryStream(provider.SharedKey);
				Array.Copy(provider.SharedKey, provider.CryptoStream.IV, provider.CryptoStream.IV.Length);

				socket.Status = (int)SocketStatus.Connected;
				return true;
			}

			return false;
		}
	}
}
