using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 특정 행동에 대한 성공 여부를 제공합니다.
	/// </summary>
	public interface IActionResult : IPacket
	{
		/// <summary>
		/// 특정 행동에 대한 성공 여부입니다.
		/// </summary>
		bool Success { get; }
	}
}
