﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IBLVM_Library.Packets;
using IBLVM_Library.Models;
using SecureStream;
using IBLVM_Library.Enums;

namespace IBLVM_Library.Interfaces
{
	/// <summary>
	/// 패킷 생성에 사용되는 팩토리 클래스 인터페이스입니다.
	/// </summary>
	public interface IPacketFactory
	{
		/// <summary>
		/// 핸드셰이크 시작 요청 패킷을 생성합니다.
		/// </summary>
		/// <returns>생성된 핸드셰이크 시작 요청 패킷입니다.</returns>
		IPacket CreateClientHello();

		/// <summary>
		/// 암호화 키 교환에 사용되는 서버 키 응답 패킷을 생성합니다.
		/// </summary>
		/// <param name="data">키 교환 과정에서 사용될 데이터입니다.</param>
		/// <returns>생성된 서버 키 응답 패킷입니다.</returns>
		IPayload<byte[]> CreateServerKeyResponse(byte[] data);

        /// <summary>
        /// 암호화 키 교환에 사용되는 클라이언트 키 응답 패킷을 생성합니다.
        /// </summary>
        /// <param name="data">키 교환 과정에서 사용될 데이터입니다.</param>
        /// <returns>생성된 클라이언트 키 응답 패킷입니다.</returns>
        IPayload<byte[]> CreateClientKeyResponse(byte[] data);

        /// <summary>
        /// 클라이언트의 로그인 요청에 대한 응답 패킷을 생성합니다.
        /// </summary>
        /// <param name="isSuccess">로그인 요청의 성공 여부입니다.</param>
        /// <returns>생성된 로그인 응답 패킷입니다.</returns>
        IPayload<bool> CreateServerLoginResponse(bool isSuccess);

		/// <summary>
		/// 서버에 로그인 하기 위한 요청 패킷을 생성합니다.
		/// </summary>
		/// <param name="id">로그인에 사용될 사용자의 식별자입니다.</param>
		/// <param name="password">로그인에 사용될 사용자의 비밀번호입니다.</param>
		/// <param name="cryptor">사용자 정보 암호화에 사용될 CryptoMemoryStream 클래스 인스턴스입니다.</param>
		/// <returns>생성된 로그인 요청 패킷입니다.</returns>
		IPayload<IAuthInfo> CreateClientLoginRequest(string id, string password, ClientType type, CryptoMemoryStream cryptor);

        /// <summary>
        /// 암호화 알고리즘에 사용되는 초기화 벡터의 변경 요청 패킷을 생성합니다.
        /// </summary>
        /// <param name="initializeVector">변경할 초기화 벡터입니다.</param>
        /// <returns>생성된 초기화 벡터 변경 요청 패킷입니다.</returns>
        IPayload<byte[]> CreateIVChangeRequest(byte[] initializeVector, CryptoMemoryStream cryptor);

        /// <summary>
        /// 암호화 알고리즘에 사용되는 초기화 벡터 변경 요청에 대한 응답 패킷을 생성합니다.
        /// </summary>
        /// <param name="isSuccess">요청에 대한 성공 여부입니다.</param>
        /// <returns>생성된 초기화 벡터 변경 요청에 대한 응답 패킷입니다.</returns>
        IPayload<bool> CreateIVChangeResposne(bool isSuccess);

        /// <summary>
        /// BitLocker 볼륨 제어 명령에 대한 응답 패킷을 생성합니다.
        /// </summary>
        /// <param name="isSuccess">명령 실행에 대한 성공 여부입니다.</param>
        /// <returns>생성된 BitLocker 볼륨 제어 명령에 대한 응답 패킷입니다.</returns>
        IPayload<bool> CreateClientBitLockerCommandResponse(bool isSuccess);

        /// <summary>
        /// 클라이언트 디바이스에 존재하는 드라이브 정보 요청 패킷을 생성합니다.
        /// </summary>
        /// <returns>생성된 디바이스 드라이브 정보 요청 패킷입니다.</returns>
        IPacket CreateServerDrivesRequest();

        /// <summary>
        /// 클라이언트 디바이스 드라이브 정보 요청에 대한 응답 패킷을 생성합니다.
        /// </summary>
        /// <param name="drives">응답으로 전송될 드라이브 정보입니다.</param>
        /// <returns>생성된 클라이언트 디바이스 드라이브 정보 요청에 대한 응답 패킷입니다.</returns>
        IPayload<DriveInformation[]> CreateClientDrivesResponse(DriveInfo[] drives);

		IPacket CreateManagerDevicesRequest();

		IPayload<ClientDrive> CreateManagerBitLockerLockRequest(ClientDrive drive);

		IPayload<ManagerBitLockerUnlock> CreateManagerBitLockerUnlockRequest(ClientDrive drive, string password, CryptoProvider cryptor);

		IPayload<IDevice> CreateManagerDrivesRequest(IDevice device);

		IPayload<IDevice[]> CreateServerDevicesResponse(IDevice[] devices);

		IPayload<ClientDrive[]> CreateServerDrivesResponse(ClientDrive[] drive);

		IPayload<DriveInformation> CreateServerBitLockerLockRequest(DriveInformation drive);

		IPayload<ServerBitLockerUnlock> CreateServerBitLockerUnlockRequest(DriveInformation drive, string password, CryptoProvider cryptor);

		IPayload<bool> CreateServerBitLockerCommandResponse(bool isSuccess);

        /// <summary>
        /// 바이트 배열에서 패킷의 헤더 부분만 파싱합니다.
        /// </summary>
        /// <param name="data">파싱할 데이터가 든 바이트 배열입니다.</param>
        /// <returns>헤더 부분의 데이터만을 가진 인터페이스를 구현하는 클래스 인스턴스입니다.</returns>
        IPacket ParseHeader(byte[] data);

		/// <summary>
		/// 패킷에 사용되는 매직 바이트입니다.
		/// </summary>
		byte[] MagicBytes { get; }

		/// <summary>
		/// 패킷 헤더 부분의 크기입니다.
		/// </summary>
		int PacketSize { get; }
	}
}
