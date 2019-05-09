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

        public override string ToString() => Volume.ToString() + ";" + Password;

        public BitLockerUnlock(BitLockerVolume volume, string password)
        {
            Volume = volume;
            Password = password;
        }
    }
}
