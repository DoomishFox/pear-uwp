using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PearLib
{
    public class PearDevice
    {
        public static ushort Size = 34;
        public PearDevice(byte[] data)
        {
            var e_id = data.Take(2);
            var e_name = data.Skip(2).Take(data[21]);
            var e_ipaddr = data.Skip(22).Take(4);
            var e_key = data.Skip(26).Take(8);

            // reverse values if endianness is little
            if (BitConverter.IsLittleEndian)
            {
                e_id = e_id.Reverse();
                e_ipaddr = e_ipaddr.Reverse();
                e_key = e_key.Reverse();
            }

            Id = BitConverter.ToUInt32(e_id.ToArray(), 0);
            Name = BitConverter.ToString(e_name.ToArray(), 0);
            IP = new IPAddress(e_ipaddr.ToArray());
            PrivateKey = BitConverter.ToUInt64(e_key.ToArray(), 0);
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public IPAddress IP { get; set; }
        public ulong PrivateKey { get; set; }
    }
}
