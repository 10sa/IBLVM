using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;

using SecureStream;

using IBLVM_Library;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Models;

namespace IBLVM_Library.Handlers
{
	public class ServerKeyResponseHandler : IPacketHandler
	{
		public bool Handle(IPacket header, IIBLVMSocket socket)
		{
			if (header.Type == PacketType.ServerKeyResponse)
			{
                Utils.PacketValidation(socket.Status, (int)ClientSocketStatus.Handshaking, header.GetPayloadSize());

                CryptoProvider cryptoProvider = socket.CryptoProvider;
				IPayload<byte[]> packet = socket.PacketFactory.CreateServerKeyResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

				cryptoProvider.SharedKey = cryptoProvider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(packet.Payload, CngKeyBlobFormat.EccPublicBlob));
				cryptoProvider.CryptoStream = new CryptoMemoryStream(cryptoProvider.SharedKey);
				byte[] nonce = new byte[cryptoProvider.CryptoStream.IV.Length];
				Array.Copy(cryptoProvider.SharedKey, nonce, nonce.Length);
				cryptoProvider.CryptoStream.IV = nonce;

				IPacket responsePacket = socket.PacketFactory.CreateClientKeyResponse(cryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				Utils.SendPacket(socket.SocketStream, responsePacket);

				socket.Status = (int)ClientSocketStatus.Connected;
				return true;
			}

			return false;
		}
	}
}
