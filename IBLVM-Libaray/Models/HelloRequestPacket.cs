using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray.Models
{
	class HelloRequestPacket : IPacket
	{
		public byte[] GetBytes()
		{
			throw new NotImplementedException();
		}
	}
}
