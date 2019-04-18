using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Server.Interfaces;

namespace IBLVM_Tests
{
	class UserValidate : IUserValidate
	{
		public bool Validate(string id, string password)
		{
			return true;
		}
	}
}
