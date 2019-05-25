using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Server.Interfaces;

namespace IBLVM_Tests
{
	class UserValidate : ISession
	{
		public IAccount Account => null;

		public bool Login(IAccount account)
		{
			Account = account;
			return true;
		}
	}
}
