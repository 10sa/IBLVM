using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Server.Interfaces;

namespace IBLVM_Tests
{
	class SessionControl : ISession
	{
		public IAccount Account { get; private set; }
		public bool Auth(IAccount account)
		{
			Account = account;
			return true;
		}
	}
}
