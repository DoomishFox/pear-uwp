using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PearLib
{
    public class Peard : IDisposable
    {
        // To detect redundant calls
        private bool _disposed;

        private string _pipeName = "peard-server";
        private NamedPipeClientStream pipe;
        
        public Peard()
        {
            pipe = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut);
            pipe.Connect();
        }

        public List<PearDevice> QueryDevices()
        {
            var devices = new List<PearDevice>();

            // query peard for a list of devices
            byte[] header = new byte[8];
            header[0] = 2;
            pipe.Write(header, 0, header.Length);

            pipe.Flush();

            // read the response
            pipe.Read(header, 0, header.Length);

            // parse the header to ensure correct response
            if (header[0] == 2 && header[1] == 1)
            {
                // device query with return flag set
                // get payload length and read devices
                var e_payloadLength = header.Skip(6).Take(2);
                if (BitConverter.IsLittleEndian)
                    e_payloadLength = e_payloadLength.Reverse();

                var payloadLength = BitConverter.ToUInt16(e_payloadLength.ToArray(), 0);

                if (payloadLength > 0)
                {
                    var deviceCount = payloadLength / PearDevice.Size;
                    while (deviceCount > 0)
                    {
                        // read device from pipe
                        byte[] deviceBytes = new byte[PearDevice.Size];
                        pipe.Read(deviceBytes, 0, deviceBytes.Length);
                            
                        devices.Add(new PearDevice(deviceBytes));

                        deviceCount--;
                    }
                }
            }

            return devices;
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {

                if (disposing)
                {
                    pipe.Close();
                    pipe.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
