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
		public  static void SendPacket(Socket socket, IPacket packet)
		{
			byte[] buffer = new byte[256];
			socket.Send(packet.GetPacketBytes());
			if (packet.GetPayloadSize() > 0)
			{
				using (Stream stream = packet.GetPayloadStream())
				{
					int readedSize;
					while ((readedSize = stream.Read(buffer, 0, buffer.Length)) > 0)
						socket.Send(buffer, readedSize, SocketFlags.None);
				}
			}
		}
	}
}
