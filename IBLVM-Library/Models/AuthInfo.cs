using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
	public class AuthInfo : IAuthInfo
	{
		public IAccount Account { get; private set; }

		public ClientType Type { get; private set; }

		public AuthInfo(IAccount account, ClientType type)
		{
			Account = account;
			Type = type;
		}

		public override string ToString()
		{
			return Account.ToString() + "," + ((byte)Type).ToString();
		}

		public static AuthInfo FromString(IAccount account, string str)
		{
			string[] datas = str.Split(',');
			return new AuthInfo(account, (ClientType)byte.Parse(datas[2]));
		}
	}
}
