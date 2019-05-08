using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using System.Security.Cryptography;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using IBLVM_Server.Enums;

using IBLVM_Library.Enums;

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

                if (header.GetPayloadSize() == 0)
                    throw new ProtocolViolationException("Protocol violation by empty payload.");

                CryptoProvider provider = socket.CryptoProvider;
				IPayload<byte[]> packet = socket.PacketFactory.CreateClientKeyResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				provider.SharedKey = provider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(packet.Payload, CngKeyBlobFormat.EccPublicBlob));
				provider.CryptoStream = new CryptoMemoryStream(provider.SharedKey);
				byte[] nonce = new byte[provider.CryptoStream.IV.Length];
				Array.Copy(provider.SharedKey, nonce, nonce.Length);
				provider.CryptoStream.IV = nonce;

				socket.Status = (int)SocketStatus.Connected;
				return true;
			}

			return false;
		}
	}
}
