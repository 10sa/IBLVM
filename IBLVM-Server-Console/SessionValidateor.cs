using IBLVM_Library.Interfaces;
using IBLVM_Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server_Console
{
	class SessionValidateor : ISession
	{
		public IAccount Account { get; private set; }

		public bool Auth(IAccount account)
		{
			Account = account;
			return true;
		}
	}
}
