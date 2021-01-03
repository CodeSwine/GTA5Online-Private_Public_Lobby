using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class IPTool
    {
        private HttpClient httpClient;

        public IPTool()
        {
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets the hosts IP Address.
        /// </summary>
        /// <returns>String value of IP.</returns>
        public async Task<string> GrabInternetAddressAsync()
        {
            // Still needs check to see if we could retrieve the IP.
            // Try for ipv6 first, but if that fails get ipv4
            string ip;
            try
            {
                var response = await this.httpClient.GetAsync(new Uri("https://ipv6.icanhazip.com"));
                ip = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                ErrorLogger.LogException(e);

                try
                {
                    var response = await this.httpClient.GetAsync(new Uri("https://ipv4.icanhazip.com"));
                    ip = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e2)
                {
                    ErrorLogger.LogException(e2);
                    ip = "IP not found.";
                }
            }
            return ip;
        }

        public static bool ValidateIP(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
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