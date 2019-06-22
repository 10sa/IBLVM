using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
    public class BitLockerUnlock
    {
        public DriveInformation Volume { get; private set; }
       
        public string Password { get; private set; }

		public BitLockerUnlock(DriveInformation volume, string password)
		{
			Volume = volume;
			Password = password;
		}

		public override string ToString() => Volume.ToString() + "," + Password;

		public static BitLockerUnlock FromString(string str)
		{
			string[] datas = str.Split(',');
			return new BitLockerUnlock(DriveInformation.FromString(str), datas.Last());
		}
    }
}
