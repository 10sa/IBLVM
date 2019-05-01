using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Models;

namespace IBLVM_Library.Interfaces
{
    /// <summary>
    /// BitLocker 볼륨 목록을 나타내는 패킷 인터페이스입니다.
    /// </summary>
	public interface IBitLockers : IPacket
	{
		BitLockerVolume[] Volumes { get; }
	}
}
