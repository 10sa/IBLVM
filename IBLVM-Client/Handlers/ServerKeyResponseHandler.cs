using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;

using SecureStream;

using IBLVM_Client.Enums;

using IBLVM_Libaray.Interfaces;
using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Models;

using IBLVM_Util;
using IBLVM_Util.Interfaces;

namespace IBLVM_Client.Handlers
{
	class ServerKeyResponseHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerKeyResponse)
			{
				if (socket.Status != (int)SocketStatus.Handshaking)
					throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

				CryptoProvider cryptoProvider = socket.CryptoProvider;
				ICryptoExchanger packet = socket.PacketFactory.CreateServerKeyResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				cryptoProvider.SharedKey = cryptoProvider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(packet.Data, CngKeyBlobFormat.EccPublicBlob));
				cryptoProvider.CryptoStream = new CryptoMemoryStream(cryptoProvider.SharedKey);
				Array.Copy(cryptoProvider.SharedKey, cryptoProvider.CryptoStream.IV, cryptoProvider.CryptoStream.IV.Length);

				IPacket responsePacket = socket.PacketFactory.CreateClientKeyResponse(cryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				SocketUtil.SendPacket(socket.GetSocketStream(), responsePacket);

				socket.Status = (int)SocketStatus.Connected;
				return true;
			}

			return false;
		}
	}
}
