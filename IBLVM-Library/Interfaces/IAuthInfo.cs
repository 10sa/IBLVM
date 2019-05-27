using IBLVM_Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Interfaces
{
	public interface IAuthInfo
	{
		IAccount Account { get; }

		ClientType Type { get; }
	}
}
