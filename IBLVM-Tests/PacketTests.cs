using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBLVM_Libaray.Models;
using IBLVM_Libaray.Enums;

namespace IBLVM_Tests
{
	[TestClass]
	public class PacketTests
	{
		[TestMethod]
		public void HelloReqeustPacketTest()
		{
			ClientHello packet = new ClientHello();
			byte[] bytes = packet.GetPacketBytes();

			Assert.IsTrue(BitConverter.ToUInt16(bytes, 0) == (ushort)PacketType.Hello);
		}

		[TestMethod]
		public void HelloResponsePacketTest()
		{
			ServerKeyResponse packet = new ServerKeyResponse();
			byte[] bytes = packet.GetPacketBytes();

			Assert.IsTrue(BitConverter.ToUInt16(bytes, 0) == (ushort)PacketType.ServerKeySend);
		}
	}
}
