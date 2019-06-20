using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace IBLVM_Library.Models
{
    public class DriveInformation
    {
        public string Name { get; private set; }

        public string VolumeLabel { get; private set; }
        
        public long TotalSize { get; private set; }

        public long TotalFreeSpace { get; private set; }

        public DriveType DriveType { get; private set;}

        public DriveInformation(string name, string volumeLabel, long totalSize, long totalFreeSpace, DriveType driveType)
        {
            Name = name;
            VolumeLabel = volumeLabel;
            TotalSize = totalSize;
            TotalFreeSpace = totalFreeSpace;
            DriveType = driveType;
        }

		public override string ToString() => $"{Name},{VolumeLabel},{TotalSize},{TotalFreeSpace},{(byte)DriveType}";

		public static DriveInformation FromString(string str)
		{
			string[] data = str.Split(',');
			return new DriveInformation(data[0], data[1], long.Parse(data[2]), long.Parse(data[3]), (DriveType)byte.Parse(data[4]));
		}

		public static explicit operator DriveInformation(DriveInfo param)
        {
            return new DriveInformation(param.Name, param.VolumeLabel, param.TotalSize, param.TotalFreeSpace, param.DriveType);
        }
    }
}
