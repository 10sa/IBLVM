using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;

namespace IBLVM_Library.Interfaces
{
    /// <summary>
    /// BitLocker 제어를 위한 명령 패킷 인터페이스입니다.
    /// </summary>
    public interface ICommand : IPacket
    {
        /// <summary>
        /// 제어 명령입니다.
        /// </summary>
        BitLockerCommand Command { get; }
    }
}
