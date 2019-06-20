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
using IBLVM_Library.Enums;

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

		public IPayload<IAuthInfo> CreateClientLoginRequest(string id, string password, ClientType type, CryptoMemoryStream cryptor) => new ClientLoginRequest(id, password, type, cryptor);

        public IPayload<byte[]> CreateIVChangeRequest(byte[] initializeVector, CryptoMemoryStream cryptor) => new IVChangeRequest(initializeVector, cryptor);

        public IPayload<bool> CreateIVChangeResposne(bool isSuccess) => new IVChangeResponse(isSuccess);

        public IPayload<bool> CreateClientBitLockerCommandResponse(bool isSuccess) => new ClientBitLockerCommandResponse(isSuccess);

        public IPayload<BitLockerUnlock> CreateBitLockerUnlockCommand(BitLockerUnlock bitlockerUnlock, CryptoMemoryStream cryptor) => new BitLockerUnlockCommand(bitlockerUnlock, cryptor);

        public IPayload<BitLockerVolume> CreateBitLockerLockCommand(BitLockerVolume volume) => new BitLockerLockCommand(volume);

        public IPacket CreateServerDrivesRequest() => new ServerDrivesRequest();

        public IPayload<DriveInformation[]> CreateClientDrivesResponse(DriveInfo[] driveInfos) => new ClientDrivesResponse(driveInfos);

		public IPacket CreateManagerDevicesRequest() => new ManagerDevicesRequest();

		public IPayload<IDevice[]> CreateServerDevicesResponse(IDevice[] devices) => new ServerDevicesResponse(devices);

		public IPacket ParseHeader(byte[] data)
        {
            int offset = 0;
            return new BasePacket(data, ref offset);
        }

		public IPayload<IDevice> CreateManagerDrivesRequest(IDevice device) => new ManagerDrivesRequest(device);

		public IPayload<DriveInformation[]> CreateServerDrivesResponse(DriveInformation[] drives) => new ServerDrivesResponse(drives);
	}
}
