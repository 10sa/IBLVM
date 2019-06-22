using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Packets
{
	class ManagerBitLockerUnlockRequest : BasePacket, IPayload<ManagerBitLockerUnlock>
	{
		public ManagerBitLockerUnlock Payload { get; private set; }

		private readonly CryptoProvider cryptoProvider;

		public ManagerBitLockerUnlockRequest(ClientDrive drive, string password, CryptoProvider cryptoProvider) : base(PacketType.ManagerBitLockerUnlock)
		{
			this.cryptoProvider = cryptoProvider;
			Payload = new ManagerBitLockerUnlock(drive, password, cryptoProvider);
		}

		public override int GetPayloadSize() => -1;

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();

			byte[] data = Encoding.UTF8.GetBytes(Payload.ToString());
			buffer.Write(data, 0, data.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			string data = Encoding.UTF8.GetString(Utils.ReadFull(stream, payloadSize));

			Payload = ManagerBitLockerUnlock.FromString(data, cryptoProvider);
		}
	}
}
