using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Libaray.Enums;
using IBLVM_Libaray.BitLocker;
using System.IO;

namespace IBLVM_Libaray.Models
{
	public class BitLockerList : BasePacket
	{
		private BitLocker.BitLocker[] bitLockerVolumes;

		public BitLockerList(BitLocker.BitLocker[] bitLockers) : base(PacketType.BitLockerList)
		{
			bitLockerVolumes = bitLockers;
		}
	}
}
