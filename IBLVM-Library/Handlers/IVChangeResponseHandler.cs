﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;
using IBLVM_Library;
using IBLVM_Library.Args;

namespace IBLVM_Library.Handlers
{
	/// <summary>
	/// 초기화 벡터 (Initialize Vector) 교환 응답 처리자 클래스입니다.
	/// </summary>
	public class IVChangeResponseHandler : IPacketHandler
    {
		public bool Handle(IPacket header, IIBLVMSocket socket)
        {
            if (header.Type == PacketType.IVChangeResponse)
            {
                IPayload<bool> result = socket.PacketFactory.CreateIVChangeResposne(false);
                result.ParsePayload(header.GetPayloadSize(), socket.SocketStream);

                if (!result.Payload)
                    throw new InvalidOperationException("IV Change request isn't accepted.");

				byte[] nextIV = socket.CryptoProvider.NextIV;
				if (nextIV != null)
					socket.CryptoProvider.CryptoStream.IV = nextIV;

				socket.CryptoProvider.NextIV = null;
				return true;
            }

            return false;
        }
    }
}
