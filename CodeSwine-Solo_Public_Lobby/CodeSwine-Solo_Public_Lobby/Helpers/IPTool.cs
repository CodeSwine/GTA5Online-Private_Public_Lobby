using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class IPTool
    {
        private string _ipAddress;

        public string IpAddress 
        {
            get 
            {
                if (_ipAddress == null) _ipAddress = GrabInternetAddress();
                return _ipAddress;
            }
        }

        /// <summary>
        /// Gets the hosts IP Address.
        /// </summary>
        /// <returns>String value of IP.</returns>
        private string GrabInternetAddress()
        {
            // Still needs check to see if we could retrieve the IP.
            string ip = "";
            try
            {
                ip = new WebClient().DownloadString("http://ipv6.icanhazip.com");
            }
            catch (Exception e)
            {
                ErrorLogger.LogException(e);
                ip = "IP not found.";
            }
            return ip;
        }

        public static bool ValidateIP(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            IPAddress address;
            if (IPAddress.TryParse(ipString, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4 
                        return true;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        // we have IPv6
                        return true;
                }
            }
            return false;
        }
    }
}
