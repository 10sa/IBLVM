using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLVM_Library.Enums;
using IBLVM_Library.Interfaces;

namespace IBLVM_Library.Models
{
    public class ServerBitLockerControl : BasePacket, ICommand
    {
        public BitLockerCommand Command { get; private set; }

        public string Arguments { get; private set; }

        public ServerBitLockerControl(BitLockerCommand command, string arguments) : base(PacketType.ServerBitLockerCommand)
        {
            Command = command;
            Arguments = arguments;
        }

        public override int GetPayloadSize() => base.GetPayloadSize() + sizeof(BitLockerCommand);

        public override Stream GetPayloadStream()
        {
            Stream stream = base.GetPayloadStream();
            byte[] cmdBytes = BitConverter.GetBytes((int)Command);
            stream.Write(cmdBytes, 0, cmdBytes.Length);

            byte[] argumentBytes = Encoding.UTF8.GetBytes(Arguments);
            stream.Write(argumentBytes, 0, argumentBytes.Length);

            return stream;
        }

        public override void ParsePayload(int payloadSize, Stream stream)
        {
            base.ParsePayload(payloadSize, stream);

        }
    }
}
