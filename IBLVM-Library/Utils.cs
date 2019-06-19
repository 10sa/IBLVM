using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Net.Sockets;
using System.Net;
using IBLVM_Library.Interfaces;
using IBLVM_Library.Enums;

namespace IBLVM_Library
{
	public static class Utils
	{
		public static byte[] ReadFull(Stream stream, int size)
		{
			byte[] buffer = new byte[size];
			ReadFull(stream, buffer, size);

			return buffer;
		}

		public static void ReadFull(Stream stream, byte[] buffer, int size)
		{
			for (int i = 0; i < size;)
				i += stream.Read(buffer, i, size - i);
		}

		public static void SendPacket(NetworkStream stream, IPacket packet)
		{
			byte[] buffer = new byte[256];
            Stream payloadStream = packet.GetPayloadStream();
            int payloadSize = packet.GetPayloadSize();
			if (payloadSize == -1)
			{
				payloadSize = (int)payloadStream.Length;
				packet.OverridePayloadSize(payloadSize);
			}

            byte[] packetData = packet.GetPacketBytes();
			stream.Write(packetData, 0, packetData.Length);
			using (payloadStream)
			{
				payloadStream.Position = 0;
				int sendBytes = 0;
				int readedSize = 0;

				while (sendBytes < payloadSize)
				{
					readedSize = payloadStream.Read(buffer, 0, Math.Min(payloadSize - sendBytes, buffer.Length));
					stream.Write(buffer, 0, readedSize);

					sendBytes += readedSize;
				}
			}
		}

        public static void PacketValidation(int socketStatus, int reqStatus, int payloadSize, bool isEmpty)
        {
            if (socketStatus != reqStatus)
                throw new ProtocolViolationException("Protocol violation by invalid packet sequence.");

            if (isEmpty)
            {
                if (payloadSize > 0)
                    throw new ProtocolViolationException("Protocol violation by unreasonable payload.");
            }
            else if (payloadSize == 0)
                throw new ProtocolViolationException("Protocol violation by empty payload.");
        }

        public static void PacketValidation(int socketStatus, int reqStatus, int payloadSize)
        {
            PacketValidation(socketStatus, reqStatus, payloadSize, false);
        }

		public static void ExchangeIV(IIBLVMSocket socket, byte[] nextIV)
		{
			socket.CryptoProvider.NextIV = nextIV;
			SendPacket(socket.SocketStream, socket.PacketFactory.CreateIVChangeRequest(nextIV, socket.CryptoProvider.CryptoStream));
		}
    }
}
