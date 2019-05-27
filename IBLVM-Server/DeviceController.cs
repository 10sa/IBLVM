using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;
using IBLVM_Server.Interfaces;

namespace IBLVM_Server
{
	class DeviceController : IDeviceController
	{
		private Dictionary<string, List<IDevice>> devices = new Dictionary<string, List<IDevice>>();

		public void AddDevice(string id, IDevice device)
		{
			if (device.Type != ClientType.Device)
				throw new ArgumentException("IDevice instansce must be ClientType.Device type.");

			if (!devices.TryGetValue(id, out List<IDevice> list))
			{
				list = new List<IDevice>();
				devices.Add(id, list);
			}

			list.Add(device);
		}

		public Dictionary<string, List<IDevice>> GetDevices() => devices;

		public IDevice[] GetUserDevices(string id)
		{
			if (devices.TryGetValue(id, out List<IDevice> list))
				return list.ToArray();
			else
				return null;
		}

		public bool RemoveDevice(string id, IDevice device) => devices[id].Remove(device);
	}
}
