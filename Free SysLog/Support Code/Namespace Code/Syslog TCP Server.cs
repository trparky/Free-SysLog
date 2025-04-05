using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Free_SysLog.SyslogTcpServer
{
    public class SyslogTcpServer
    {
        private readonly int _port;
        private readonly Delegate _syslogMessageHandler;
        private TcpListener TCPListener;
        private bool boolLoopControl = true;

        public SyslogTcpServer(Delegate syslogMessageHandler, int port = 514)
        {
            _port = port;
            _syslogMessageHandler = syslogMessageHandler;
        }

        public async Task StartAsync()
        {
            try
            {
                // These are initialized as IPv4 mode.
                var ipAddressSetting = IPAddress.Any;

                if (My.MySettingsProperty.Settings.IPv6Support)
                    ipAddressSetting = IPAddress.IPv6Any;

                TCPListener = new TcpListener(ipAddressSetting, _port);
                if (My.MySettingsProperty.Settings.IPv6Support)
                    TCPListener.Server.DualMode = true;
                TCPListener.Start();

                while (boolLoopControl)
                {
                    var tcpClient = await TCPListener.AcceptTcpClientAsync();
                    await HandleClientAsync(tcpClient);
                }
            }
            catch (Exception ex)
            {
                _syslogMessageHandler.DynamicInvoke($"Exception Type: {ex.GetType()}{Constants.vbCrLf}Exception Message: {ex.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{ex.StackTrace}", IPAddress.Loopback.ToString());
            }
        }

        private async Task HandleClientAsync(TcpClient tcpClient)
        {
            using (tcpClient)
            {
                IPEndPoint remoteIPEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;

                using (var stream = tcpClient.GetStream())
                {
                    var dataBuffer = new byte[4096];
                    int intBytesRead;
                    string strMessage;

                    try
                    {
                        do
                        {
                            intBytesRead = await stream.ReadAsync(dataBuffer, 0, dataBuffer.Length);

                            if (intBytesRead != 0)
                            {
                                strMessage = Encoding.UTF8.GetString(dataBuffer, 0, intBytesRead).Trim();

                                if (strMessage.Equals(SupportCode.SupportCode.strTerminate, StringComparison.OrdinalIgnoreCase))
                                {
                                    TCPListener.Stop();
                                    boolLoopControl = false;
                                    break;
                                }
                                else
                                {
                                    _syslogMessageHandler.DynamicInvoke(strMessage, SupportCode.SupportCode.GetIPv4Address(remoteIPEndPoint.Address).ToString());
                                }
                            }
                        }
                        while (intBytesRead != 0);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}