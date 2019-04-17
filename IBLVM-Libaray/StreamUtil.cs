using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace IBLVM_Libaray
{
	public static class StreamUtil
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
	}
}
