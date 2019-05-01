using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

using SecureStream;
using System.IO;

namespace IBLVM_Library.Models
{
    public class BitLockerCommandLock : BasePacket
    {
        public BitLockerCommandLock() : base(PacketType.ServerBitLockerLockCommand) { }
    }
}
