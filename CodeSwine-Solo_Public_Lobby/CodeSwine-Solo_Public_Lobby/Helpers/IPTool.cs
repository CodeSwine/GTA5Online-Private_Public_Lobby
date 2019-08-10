using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
                ip = new WebClient().DownloadString("http://ipv4.icanhazip.com");
            }
            catch (Exception e)
            {
                ErrorLogger.LogException(e);
                ip = "IP not found.";
            }
            return ip;
        }

        /// <summary>
        /// Validates IP Address.
        /// </summary>
        /// <param name="ipString"></param>
        /// <returns>Bool True or False.</returns>
        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');

            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public IEnumerable<IPAddress> LanIPAddresses
        {
            get
            {
                var result = new List<IPAddress>();
                var lanIPAddress = GetLanIPAddress();
                if (ValidateIPv4(lanIPAddress))
                {
                    var baseIP = string.Join(".", lanIPAddress.Split('.').Take(3));
                    for (var i = 1; i < 255; i++)
                    {
                        result.Add(IPAddress.Parse(string.Format("{0}.{1}", baseIP, i)));
                    }
                }

                return result;
            }
        }

        private static string GetLanIPAddress()
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in hostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            return string.Empty;
        }
    }
}
