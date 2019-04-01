using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Libaray.Models
{
	class ExchangeInitalizeVector : BasePacket
	{
		private readonly byte[] initializeVector;

		public ExchangeInitalizeVector(byte[] initializeVector) : base(Enums.PacketType.ExchangeInitializeVector)
		{
			this.initializeVector = initializeVector;
		}

		protected override void CreateBytes(Stream buffer)
		{
			base.CreateBytes(buffer);
			buffer.Write(BitConverter.GetBytes(initializeVector.Length), 0, sizeof(int));
			buffer.Write(initializeVector, 0, initializeVector.Length);
		}


		public override Stream GetPayloadStream() => null;
	}
}
