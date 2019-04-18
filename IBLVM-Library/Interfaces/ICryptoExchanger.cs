using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 암호화와 관련된 데이터의 교환을 위한 패킷 인터페이스입니다.
	/// </summary>
	public interface ICryptoExchanger : IPacket
	{
		/// <summary>
		/// 암호화와 관련된 데이터입니다.
		/// </summary>
		byte[] Data { get; }
	}
}
