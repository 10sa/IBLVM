using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBLVM_Libaray.Models;
using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Factories;
using IBLVM_Libaray.Interfaces;

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

			Assert.IsTrue(packet.Type == PacketType.Hello);
		}

		[TestMethod]
		public void ServerKeyResponseTest()
		{
			byte[] bytes = packetFactroy.CreateServerKeyResponse(new byte[0]).GetPacketBytes();
			IPacket packet = packetFactroy.ParseHeader(bytes);

			Assert.IsTrue(packet.Type == PacketType.ServerKeySend);
		}
	}
}
