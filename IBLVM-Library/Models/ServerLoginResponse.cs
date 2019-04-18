using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;

using IBLVM_Library.Enums;

namespace IBLVM_Library.Models
{
	public sealed class ServerLoginResponse : BasePacket
	{
		public bool Success { get; private set; }

		public ServerLoginResponse(bool isSuccess) : base(PacketType.ServerLoginResponse)
		{
			Success = isSuccess;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + sizeof(bool);

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			byte[] success = BitConverter.GetBytes(Success);
			buffer.Write(success, 0, success.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] buffer = new byte[sizeof(bool)];
			stream.Read(buffer, 0, buffer.Length);

			Success = BitConverter.ToBoolean(buffer, 0);
		}
	}
}
