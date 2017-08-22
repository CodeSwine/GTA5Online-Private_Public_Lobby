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
        public string IpAddress 
        {
            get 
            {
                return GrabInternetAddress();
            }
        }

        /// <summary>
        /// Gets the hosts IP Address.
        /// </summary>
        /// <returns>String value of IP.</returns>
        public string GrabInternetAddress()
        {
            // Still needs check to see if we could retrieve the IP. 
            return new WebClient().DownloadString("http://icanhazip.com");
        }

        /// <summary>
        /// Validates IP Address.
        /// </summary>
        /// <param name="ipString"></param>
        /// <returns>Bool True or False.</returns>
        public bool ValidateIPv4(string ipString)
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
    }
}
