using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	public class ClientKeyResponse : BasePacket
	{
		private readonly byte[] cryptoKey;

		public ClientKeyResponse(byte[] cryptoKey) : base(PacketType.ClientKeySend)
		{
			this.cryptoKey = cryptoKey;
		}

		protected override void CreateBytes(Stream buffer)
		{
			base.CreateBytes(buffer);
			buffer.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));
			buffer.Write(cryptoKey, 0, cryptoKey.Length);
		}

		public override Stream GetPayloadStream() => null;
	}
}
