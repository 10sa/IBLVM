using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Library.Handlers;
using IBLVM_Library;
using IBLVM_Management.Handlers;
using IBLVM_Library.Models;

namespace IBLVM_Management
{
	class ManagerHandlerChain
	{
		private readonly PacketHandlerChain chain;
		private readonly ServerDevicesResponseHandler devicesResponseHandler;
		private readonly ServerDrivesResponseHandler drivesResponseHandler;
		private readonly ServerBitLockerCommandResponseHandler bitLockerCommandResponseHandler;

		public event Action<IDevice[]> OnDevicesReceived
		{
			add
			{
				devicesResponseHandler.OnDevicesReceived += value;
			}

			remove
			{
				devicesResponseHandler.OnDevicesReceived -= value;
			}
		}

		public event Action<ClientDrive[]> OnDrivesReceived
		{
			add
			{
				drivesResponseHandler.OnDrivesReceived += value;
			}

			remove
			{
				drivesResponseHandler.OnDrivesReceived -= value;
			}
		}

		public event Action<bool> OnBitLockerCommandResponseReceived {
			add {
				bitLockerCommandResponseHandler.OnBitLockerCommandResponseReceived += value;
			}

			remove {
				bitLockerCommandResponseHandler.OnBitLockerCommandResponseReceived -= value;
			}
		}

		public ManagerHandlerChain(IIBLVMSocket socket)
		{
			chain = new PacketHandlerChain(socket);
			chain.AddHandler(new IVChangeRequestHandler());
			chain.AddHandler(new IVChangeResponseHandler());
			chain.AddHandler(new ServerKeyResponseHandler());
			chain.AddHandler(new ServerLoginResponseHandler());
			devicesResponseHandler = new ServerDevicesResponseHandler();
			chain.AddHandler(devicesResponseHandler);

			drivesResponseHandler = new ServerDrivesResponseHandler();
			chain.AddHandler(drivesResponseHandler);

			bitLockerCommandResponseHandler = new ServerBitLockerCommandResponseHandler();
			chain.AddHandler(bitLockerCommandResponseHandler);
		}

		public bool DoHandle(IPacket header) => chain.DoHandle(header);
	}
}
