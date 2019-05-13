using System.Net;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
{
	public interface IDevice
	{
		IAccount Account { get; }

		IPAddress DeviceIP { get; }
	}
}