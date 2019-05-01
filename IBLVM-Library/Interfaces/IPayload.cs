using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
    /// <summary>
    /// 패킷이 페이로드를 가지고 있음을 나타내는 인터페이스입니다.
    /// </summary>
    /// <typeparam name="T">페이로드의 자료형입니다.</typeparam>
    public interface IPayload<T> : IPacket
    {
        T Payload { get; }
    }
}
