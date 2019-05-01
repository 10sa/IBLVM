﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;

using SecureStream;

using IBLVM_Client.Enums;

using IBLVM_Library;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library.Models;

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
				IPayload<byte[]> packet = socket.PacketFactory.CreateServerKeyResponse(null);
				packet.ParsePayload(header.GetPayloadSize(), socket.GetSocketStream());

				cryptoProvider.SharedKey = cryptoProvider.ECDiffieHellman.DeriveKeyMaterial(CngKey.Import(packet.Payload, CngKeyBlobFormat.EccPublicBlob));
				cryptoProvider.CryptoStream = new CryptoMemoryStream(cryptoProvider.SharedKey);
				byte[] nonce = new byte[cryptoProvider.CryptoStream.IV.Length];
				Array.Copy(cryptoProvider.SharedKey, nonce, nonce.Length);
				cryptoProvider.CryptoStream.IV = nonce;

				IPacket responsePacket = socket.PacketFactory.CreateClientKeyResponse(cryptoProvider.ECDiffieHellman.PublicKey.ToByteArray());
				Utils.SendPacket(socket.GetSocketStream(), responsePacket);

				socket.Status = (int)SocketStatus.Connected;
				return true;
			}

			return false;
		}
	}
}
