using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CryptoStream;
using System.Net.Sockets;

namespace IBLVM_Util.Interfaces
{
	public interface IIBLVMSocket
	{
		void SetSocketStatus(int status);

		NetworkStream GetSocketStream();

		CryptoMemoryStream GetCryptoStream();
	}
}
