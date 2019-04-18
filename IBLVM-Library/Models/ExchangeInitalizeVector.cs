using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
	public sealed class ExchangeInitalizeVector : BasePacket
	{
		private readonly byte[] initializeVector;

		public ExchangeInitalizeVector(byte[] initializeVector) : base(Enums.PacketType.ExchangeInitializeVector)
		{
			this.initializeVector = initializeVector;
		}

		public override int GetPayloadSize() => base.GetPayloadSize() + initializeVector.Length;
	}
}
