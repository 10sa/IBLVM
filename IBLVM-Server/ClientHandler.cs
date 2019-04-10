using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using CryptoStream;

namespace IBLVM_Server
{
	class ClientHandler
	{
		public Thread Thread;

		private CryptoMemoryStream cryptoStream;
		private Socket socket;

		public ClientHandler(Socket socket)
		{
			this.socket = socket;
		}

		public void Start()
		{
			Thread = new Thread(() =>
			{

			});
		}
	}
}
