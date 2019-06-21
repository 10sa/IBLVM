using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBLVM_Library.Packets;
using IBLVM_Library.Models;
using IBLVM_Library.Enums;
using IBLVM_Library.Factories;
using IBLVM_Library.Interfaces;

using System.IO;

namespace IBLVM_Tests
{
	[TestClass]
	public class PacketTests
	{
		private PacketFactroy packetFactroy = new PacketFactroy();
		
		[TestMethod]
		public void ClientHelloTest()
		{
			byte[] bytes = packetFactroy.CreateClientHello().GetPacketBytes();
			IPacket packet = packetFactroy.ParseHeader(bytes);

			Assert.IsTrue(packet.Type == PacketType.ClientHello);
		}

		[TestMethod]
		public void ServerKeyResponseTest()
		{
			byte[] bytes = packetFactroy.CreateServerKeyResponse(new byte[0]).GetPacketBytes();
			IPacket packet = packetFactroy.ParseHeader(bytes);

			Assert.IsTrue(packet.Type == PacketType.ServerKeyResponse);
		}

		
		[TestMethod]
		public void ClientLoginRequestTest()
		{
			byte[] cryptoKey = new byte[32];
			CryptoProvider provider = new CryptoProvider();
			provider.CryptoStream = new SecureStream.CryptoMemoryStream(cryptoKey);

			ClientLoginRequest packet = new ClientLoginRequest("Testing", "Password", ClientType.Device, provider.CryptoStream);

			Stream payload = packet.GetPayloadStream();
			payload.Position = 0;

			ClientLoginRequest parsedPacket = new ClientLoginRequest(null, null, 0, provider.CryptoStream);
			parsedPacket.ParsePayload(packet.GetPayloadSize(), payload);

			Assert.IsTrue(packet.Payload.Account.Id == parsedPacket.Payload.Account.Id && packet.Payload.Account.Password == parsedPacket.Payload.Account.Password);
		}
	}
}
