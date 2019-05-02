using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Packets;
using IBLVM_Library.Models;

using SecureStream;

namespace IBLVM_Library.Factories
{
	public class PacketFactroy : IPacketFactory
	{
		public byte[] MagicBytes => BasePacket.MagicBytes;

		public int PacketSize => BasePacket.GetHeaderSize();

		public IPacket CreateClientHello() => new ClientHello();

		public IPayload<byte[]> CreateClientKeyResponse(byte[] data) => new ClientKeyResponse(data);

		public IPayload<byte[]> CreateServerKeyResponse(byte[] data) => new ServerKeyResponse(data);

		public IPayload<bool> CreateServerLoginResponse(bool isSuccess) => new ServerLoginResponse(isSuccess);

		public IPayload<IAccount> CreateClientLoginRequest(string id, string password, CryptoMemoryStream cryptor) => new ClientLoginRequest(id, password, cryptor);

        public IPayload<BitLockerVolume[]> CreateClientBitLockersResponse(BitLocker[] volumes)
        {
            List<BitLockerVolume> bitLockers = new List<BitLockerVolume>();
            foreach (BitLocker locker in volumes)
                bitLockers.Add(new BitLockerVolume(locker.DeviceID, locker.DriveLetter));

            return new ClientBitLockersResponse(bitLockers.ToArray());
        }

        public IPacket CreateServerBitLockersReqeust() => new ServerBitLockersRequest();

        public IPayload<byte[]> CreateIVChangeRequest(byte[] initializeVector) => new IVChangeRequest(initializeVector);

        public IPayload<bool> CreateIVChangeResposne(bool isSuccess) => new IVChangeResponse(isSuccess);

        public IPayload<bool> CreateClientBitLockerCommandResponse(bool isSuccess) => new ClientBitLockerCommandResponse(isSuccess);

        public IPayload<string> CreateBitLockerUnlockCommand(string password, CryptoMemoryStream cryptor) => new BitLockerUnlockCommand(password, cryptor);

        public IPacket CreateBitLockerLockCommand() => new BitLockerLockCommand();

        public IPacket CreateServerDrivesRequest() => new ServerDrivesRequest();

        public IPayload<DriveInfomation[]> CreateClientDrivesResponse(DriveInfo[] driveInfos) => new ClientDrivesResponse(driveInfos);

        public IPacket ParseHeader(byte[] data)
        {
            int offset = 0;
            return new BasePacket(data, ref offset);
        }
    }
}
