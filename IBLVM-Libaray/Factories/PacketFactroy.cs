using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;
using IBLVM_Libaray.Models;

using SecureStream;

namespace IBLVM_Libaray.Factories
{
	public class PacketFactroy : IPacketFactory
	{
		public byte[] MagicBytes => BasePacket.MagicBytes;

		public int PacketSize => BasePacket.GetHeaderSize();

		public IPacket CreateClientHello() => new ClientHello();

		public IPacket CreateClientKeyResponse(byte[] cryptoKey) => new ClientKeyResponse(cryptoKey);

		public IPacket CreateServerKeyResponse(byte[] cryptoKey) => new ServerKeyResponse(cryptoKey);

		public IPacket CreateServerLoginResponse(bool isSuccess) => new ServerLoginResponse(isSuccess);

		public IPacket CreateClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) => new ClientLoginRequest(id, password, cryptor);

		public IPacket ParseHeader(byte[] data)
		{
			int offset = 0;
			return new BasePacket(data, ref offset);
		}
	}
}
