using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class RangeCalculator
    {
        public static string GetRemoteAddresses(List<IPAddress> addresses)
        {
            SortedDictionary<UInt32, IPAddress> ipDictionary = new SortedDictionary<UInt32, IPAddress>();
            foreach (IPAddress address in addresses)
            {
                ipDictionary.Add(GetIntFromIp(address), address);
            }
            List<IPAddress> addresslist = ipDictionary.Select(kvp => kvp.Value).ToList();
            return ConstructRange(addresslist);
        }

        private static UInt32 GetIntFromIp(IPAddress address)
        {
            byte[] ip = address.GetAddressBytes();
            if (BitConverter.IsLittleEndian) Array.Reverse(ip);
            return BitConverter.ToUInt32(ip, 0);
        }

        private static IPAddress Add(IPAddress address)
        {
            byte[] newBytes = new IPAddress(GetIntFromIp(address) + 1).GetAddressBytes();
            Array.Reverse(newBytes);
            return new IPAddress(newBytes);
        }

        private static IPAddress Subtract(IPAddress address)
        {
            byte[] newBytes = new IPAddress(GetIntFromIp(address) - 1).GetAddressBytes();
            Array.Reverse(newBytes);
            return new IPAddress(newBytes);
        }

        /// <summary>
        /// Constructs the string remoteAddresses.
        /// </summary>
        /// <param name="list">List with IPAddresses.</param>
        private static string ConstructRange(List<IPAddress> list)
        {
            if (list.Count > 0)
            {
                var scope = new StringBuilder("1.1.1.1-");
                var isContiguous = false;
                for (var i = 0; i < list.Count; i++)
                {
                    var ip = list[i];

                    if (!isContiguous)
                    {
                        scope.Append(Subtract(ip));
                        scope.Append(",");
                    }
                    isContiguous = i < list.Count - 1 && Add(ip).ToString() == list[i+1].ToString();

                    if (!isContiguous)
                    {
                        scope.Append(Add(ip));
                        scope.Append("-");
                    }
                }
                scope.Append("255.255.255.254");
                return scope.ToString();
            }
            return "";
        }
    }
}