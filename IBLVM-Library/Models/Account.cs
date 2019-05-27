using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
{
    public class Account : IAccount
    {
        public Account(string id, string password)
        {
            Id = id;
            Password = password;
        }

        public string Id { get; private set; }

        public string Password { get; private set; }

		public override string ToString()
		{
			return Id + "," + Password;
		}

		public static Account FromString(string str)
		{
			string[] datas = str.Split(',');
			return new Account(datas[0], datas[1]);
		}
	}
}
