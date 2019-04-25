using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Enums;

namespace IBLVM_Library.Interfaces
{
    /// <summary>
    /// BitLocker 제어를 위한 명령을 나타냅니다.
    /// </summary>
    public interface ICommand : IPacket
    {
        /// <summary>
        /// 제어 명령입니다.
        /// </summary>
        BitLockerCommand Command { get; }

        /// <summary>
        /// 제어 명령에 대한 인자입니다.
        /// </summary>
        string Arguments { get; }
    }
}
