using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
    /// <summary>
    /// 서버에 로그인하기 위한 정보를 나타내는 인터페이스입니다.
    /// </summary>
    public interface IAccount
    {
        string Id { get; }

        string Password { get; }
    }
}
