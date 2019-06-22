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
	public class ServerBitLockerUnlockRequest : BasePacket, IPayload<ServerBitLockerUnlock>
	{
		public ServerBitLockerUnlock Payload { get; private set; }

		private CryptoProvider cryptor;

		public ServerBitLockerUnlockRequest(DriveInformation drive, string password,CryptoProvider cryptor) : base(PacketType.ServerBitLockerUnlockRequest)
		{
			Payload = new ServerBitLockerUnlock(drive, password, cryptor);
			this.cryptor = cryptor;
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

			Payload = ServerBitLockerUnlock.FromString(data, cryptor);
		}
	}
}
