using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net.Sockets;

using IBLVM_Libaray.Interfaces;

namespace IBLVM_Util
{
    public static class SocketUtil
    {
		public  static void SendPacket(NetworkStream stream, IPacket packet)
		{
			byte[] buffer = new byte[256];
			byte[] packetData = packet.GetPacketBytes();

			stream.Write(packetData, 0, packetData.Length);
			if (packet.GetPayloadSize() > 0)
			{
				using (Stream payloadStream = packet.GetPayloadStream())
				{
					int readedSize;
					while ((readedSize = payloadStream.Read(buffer, 0, buffer.Length)) > 0)
						stream.Write(buffer, 0, readedSize);
				}
			}
		}

		public static byte[] ReceiveFull(NetworkStream stream, int size)
		{
			byte[] buffer = new byte[size];
			ReceiveFull(stream, buffer, size);

			return buffer;
		}

		public static void ReceiveFull(NetworkStream stream, byte[] buffer, int size)
		{
			for (int i = 0; i < size;)
				i += stream.Read(buffer, i, size - i);
		}
	}
}
