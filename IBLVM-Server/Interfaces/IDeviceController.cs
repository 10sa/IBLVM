using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBLVM_Library.Interfaces;
using IBLVM_Server.Interfaces;

namespace IBLVM_Server.Interfaces
{
	public interface IDeviceController
	{
		IDevice[] GetUserDevices(string id);

		Dictionary<string, List<IDevice>> GetDevices();

		void AddDevice(string id, IDevice device);

		bool RemoveDevice(string id, IDevice device);
	}
}
