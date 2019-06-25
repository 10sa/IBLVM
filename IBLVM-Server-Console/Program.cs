using IBLVM_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IBLVM_Server_Console
{
	class Program
	{
		static void Main(string[] args)
		{
			IBLVMServer server = new IBLVMServer(new SessionValidateor());
			server.Bind(new IPEndPoint(IPAddress.Any, 54541));
			server.Listen(5);
			server.Start();

			server.ServerThread.Join();
		}
	}
}
