using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Args
{
	/// <summary>
	/// 초기화 벡터 (Initialize Vector) 교환 요청을 승인할지에 대한 이벤트의 인자입니다.
	/// </summary>
	public class IVExchangeAcceptEventArgs : EventArgs
	{
		public bool Accpet { get; set; } = true;
	}
}
