using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace PearDiscoveryServer
{
    public sealed class DiscoveryListener : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as SocketActivityTriggerDetails;
            var socketInformation = details.SocketInformation;
            switch (details.Reason)
            {
                case SocketActivityTriggerReason.SocketActivity:
                    var socket = socketInformation.StreamSocket;
                    DataReader reader = new DataReader(socket.InputStream);
                    reader.InputStreamOptions = InputStreamOptions.Partial;
                    await reader.LoadAsync(250);
                    var dataString = reader.ReadString(reader.UnconsumedBufferLength);
                    //ShowToast(dataString);
                    socket.TransferOwnership(socketInformation.Id); /* Important! */
                    break;
                case SocketActivityTriggerReason.KeepAliveTimerExpired:
                    socket = socketInformation.StreamSocket;
                    DataWriter writer = new DataWriter(socket.OutputStream);
                    writer.WriteBytes(Encoding.UTF8.GetBytes("Keep alive"));
                    await writer.StoreAsync();
                    writer.DetachStream();
                    writer.Dispose();
                    socket.TransferOwnership(socketInformation.Id); /* Important! */
                    break;
            }

            _deferral.Complete();
        }
    }
}
