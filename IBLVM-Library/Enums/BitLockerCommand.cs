using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Enums
{
    /// <summary>
    /// BitLocker 볼륨에 대한 제어 명령들을 나타냅니다.
    /// </summary>
    public enum BitLockerCommand : int
    {
        /// <summary>
        /// BitLocker 볼륨의 키를 해제하고 잠금 상태로 전환합니다. 이 기능은 디스크에 실행 파일이 위치한 프로세스들에 대한 안정성을 제공하지 않습니다.
        /// </summary>
        Lock,

        /// <summary>
        /// 잠겨있는 BitLocker 볼륨의 잠금을 해제합니다.
        /// </summary>
        Unlock
    }
}
