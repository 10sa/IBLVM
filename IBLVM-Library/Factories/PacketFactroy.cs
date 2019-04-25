using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Models;

using SecureStream;

namespace IBLVM_Library.Factories
{
	public class PacketFactroy : IPacketFactory
	{
		public byte[] MagicBytes => BasePacket.MagicBytes;

		public int PacketSize => BasePacket.GetHeaderSize();

		public IPacket CreateClientHello() => new ClientHello();

		public ICryptoExchanger CreateClientKeyResponse(byte[] data) => new ClientKeyResponse(data);

		public ICryptoExchanger CreateServerKeyResponse(byte[] data) => new ServerKeyResponse(data);

		public IActionResult CreateServerLoginResponse(bool isSuccess) => new ServerLoginResponse(isSuccess);

		public IAuthentication CreateClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) => new ClientLoginRequest(id, password, cryptor);

        public IBitLockers CreateClientBitLockersResponse(BitLocker[] volumes)
        {
            List<BitLockerVolume> bitLockers = new List<BitLockerVolume>();
            foreach (BitLocker locker in volumes)
                bitLockers.Add(new BitLockerVolume(locker.DeviceID, locker.DriveLetter));


            return new ClientBitLockersResponse(bitLockers.ToArray());
        }

        public IPacket CreateServerBitLockersReqeust() => new ServerBitLockersRequest();

        public ICryptoExchanger CreateIVChangeRequest(byte[] initializeVector) => new IVChangeRequest(initializeVector);

        public IActionResult CreateIVChangeResposne(bool isSuccess) => new IVChangeResponse(isSuccess);

        public ICommand CreateServerBitLockerControl(BitLockerCommand command, params object[] arguments)
        {

            return null;
        }

        public IPacket ParseHeader(byte[] data)
		{
			int offset = 0;
			return new BasePacket(data, ref offset);
		}
    }
}
