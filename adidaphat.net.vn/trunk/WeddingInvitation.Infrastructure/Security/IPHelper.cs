using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WeddingInvitation.Infrastructure.Security
{
    public class IPHelper
    {
        public static string ConvertToIPRange(string ipAddress)
        {
            try
            {
                var ipArray = ipAddress.Split('.');
                var number = ipArray.Length;
                double ipRange = 0;
                if (number != 4)
                {
                    return "error ipAddress";
                }
                for (var i = 0; i < 4; i++)
                {
                    var numPosition = int.Parse(ipArray[3 - i]);
                    if (i == 4)
                    {
                        ipRange += numPosition;
                    }
                    else
                    {
                        ipRange += ((numPosition % 256) * (Math.Pow(256, (i))));
                    }
                }
                return ipRange.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                var hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (var hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    if (localIPs.Contains(hostIP))
                    {
                        return true;
                    }
                }
            }
            catch
            { }
            return false;
        }
    }
}
