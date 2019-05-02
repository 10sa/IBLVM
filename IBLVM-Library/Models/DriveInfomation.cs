using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace IBLVM_Library.Models
{
    public class DriveInfomation
    {
        public string Name { get; private set; }

        public string VolumeLabel { get; private set; }
        
        public long TotalSize { get; private set; }

        public long TotalFreeSpace { get; private set; }

        public DriveType DriveType { get; private set;}

        public DriveInfomation(string name, string volumeLabel, long totalSize, long totalFreeSpace, DriveType driveType)
        {
            Name = name;
            VolumeLabel = volumeLabel;
            TotalSize = totalSize;
            TotalFreeSpace = totalFreeSpace;
            DriveType = driveType;
        }

        public static explicit operator DriveInfomation(DriveInfo param)
        {
            return new DriveInfomation(param.Name, param.VolumeLabel, param.TotalSize, param.TotalFreeSpace, param.DriveType);
        }
    }
}
