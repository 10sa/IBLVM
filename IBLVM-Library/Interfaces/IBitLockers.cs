using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Models;

namespace IBLVM_Library.Interfaces
{
	public interface IBitLockers : IPacket
	{
		BitLockerVolume[] Volumes { get; }
	}
}
