using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using IBLVM_Library.Enums;

namespace IBLVM_Library.Models
{
    public class DriveInformation
    {
        public string Name { get; private set; }

        public string VolumeLabel { get; private set; }
        
        public long TotalSize { get; private set; }

        public long TotalFreeSpace { get; private set; }

        public DriveType DriveType { get; private set;}

		public bool IsBitLocker { get; private set; }

		public bool IsProtected { get; private set; }

        public DriveInformation(string name, string volumeLabel, long totalSize, long totalFreeSpace, DriveType driveType)
        {
            Name = name;
            VolumeLabel = volumeLabel;
            TotalSize = totalSize;
            TotalFreeSpace = totalFreeSpace;
            DriveType = driveType;

			BitLocker bitlocker = BitLocker.GetVolume(Name);
			if (bitlocker != null)
			{
				IsBitLocker = true;
				IsProtected = bitlocker.GetProtectionStatus() == ProtectionStatus.Unknown;
			}
		}

		public DriveInformation(string name, string volumeLabel, long totalSize, long totalFreeSpace, DriveType driveType, bool isBitLocker, bool isProtected) : this(name, volumeLabel, totalSize, totalFreeSpace, driveType)
		{
			IsBitLocker = isBitLocker;
			IsProtected = isProtected;
		}

		public override string ToString() => $"{Name},{VolumeLabel},{TotalSize},{TotalFreeSpace},{(byte)DriveType},{BitConverter.GetBytes(IsBitLocker)},{BitConverter.GetBytes(IsProtected)}";

		public static DriveInformation FromString(string str)
		{
			string[] data = str.Split(',');
			return new DriveInformation(data[0], data[1], long.Parse(data[2]), long.Parse(data[3]), (DriveType)byte.Parse(data[4]), Convert.ToBoolean(byte.Parse(data[5])), Convert.ToBoolean(byte.Parse(data[6])));
		}

		public static explicit operator DriveInformation(DriveInfo param)
        {
            return new DriveInformation(param.Name, param.VolumeLabel, param.TotalSize, param.TotalFreeSpace, param.DriveType);
        }
    }
}
