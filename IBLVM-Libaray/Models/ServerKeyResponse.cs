using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	sealed class ServerKeyResponse : BasePacket
	{
		private readonly byte[] cryptoKey;

		public ServerKeyResponse(byte[] cryptoKey) : base(Enums.PacketType.ServerKeySend)
		{
			this.cryptoKey = cryptoKey;
		}

		public ServerKeyResponse(byte[] data, ref int offset) : base(data, ref offset)
		{
			cryptoKey = new byte[data.Length - offset];
			Array.Copy(cryptoKey, 0, data, offset, data.Length - offset);
		}

		protected override void CreateBytes(Stream buffer)
		{
			base.CreateBytes(buffer);
			buffer.Write(BitConverter.GetBytes(cryptoKey.Length), 0, sizeof(int));
			buffer.Write(cryptoKey, 0, cryptoKey.Length);
		}

		public override Stream GetPayloadStream() => null;
	}
}
