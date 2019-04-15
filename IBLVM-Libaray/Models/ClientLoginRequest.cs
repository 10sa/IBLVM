using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;

namespace IBLVM_Libaray.Models
{
	class ClientLoginRequest : BasePacket
	{
		public string ID { get; private set; }

		public string Password { get; private set; }

		public ClientLoginRequest(string ID, string password) : base(PacketType.ClientLoginRequest)
		{
			this.ID = ID;
			this.Password = password;
		}

		public override int GetPayloadSize() => Encoding.UTF8.GetByteCount(ID) + Encoding.UTF8.GetByteCount(Password);

		public override Stream GetPayloadStream()
		{
			Stream buffer = base.GetPayloadStream();
			byte[] id = Encoding.UTF8.GetBytes(ID);
			byte[] password = Encoding.UTF8.GetBytes(Password);

			buffer.Write(id, 0, id.Length);
			buffer.Write(password, 0, password.Length);

			return buffer;
		}

		public override void ParsePayload(int payloadSize, Stream stream)
		{
			base.ParsePayload(payloadSize, stream);
			byte[] buffer = new byte[512]; // Maximun 256 character (utf8)

			stream.Read(buffer, 0, buffer.Length);
			int paritionIndex = Array.IndexOf(buffer, 0x0);

			ID = Encoding.UTF8.GetString(buffer, 0, paritionIndex);
			Password = Encoding.UTF8.GetString(buffer, paritionIndex + 1, buffer.Length - paritionIndex - 1);
		}
	}
}
