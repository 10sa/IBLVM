using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Net.Sockets;
using IBLVM_Libaray.Interfaces;

namespace IBLVM_Libaray
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
			byte[] packetData = packet.GetPacketBytes();
			int payloadSize = packet.GetPayloadSize();

			stream.Write(packetData, 0, packetData.Length);
			using (Stream payloadStream = packet.GetPayloadStream())
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
	}
}
