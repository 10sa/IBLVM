using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Library.Models
{
    public class BitLockerUnlock
    {
        public BitLockerVolume Volume { get; private set; }
       
        public string Password { get; private set; }

		public BitLockerUnlock(BitLockerVolume volume, string password)
		{
			Volume = volume;
			Password = password;
		}

		public override string ToString() => Volume.ToString() + "," + Password;

		public static BitLockerUnlock FromString(string str)
		{
			string[] datas = str.Split(',');
			return new BitLockerUnlock(BitLockerVolume.FromString(str), datas.Last());
		}
    }
}
