using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using System.IO;


namespace IBLVM_Library.Packets
{
	public sealed class ClientBitLockersResponse : BasePacket, IPayload<BitLockerVolume[]>
	{
		public BitLockerVolume[] Payload { get; private set; }

		public ClientBitLockersResponse(BitLockerVolume[] bitLockers) : base(PacketType.ClientBitLockersResponse)
		{
			Payload = bitLockers;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] datas = Utils.ReadFull(stream, payloadSize);

			List<BitLockerVolume> volumes = new List<BitLockerVolume>();
			foreach (string volumeInfo in Encoding.UTF8.GetString(datas).Split(';'))
				volumes.Add(BitLockerVolume.FromString(volumeInfo));

			Payload = volumes.ToArray();
		}

		public override Stream GetPayloadStream()
		{
			Stream stream = base.GetPayloadStream();
			foreach (var bitlocker in Payload)
			{
				byte[] data = Encoding.UTF8.GetBytes(bitlocker.ToString());
				stream.Write(data, 0, data.Length);
			}

			return stream;
		}

		public override int GetPayloadSize() => -1;
	}
}
