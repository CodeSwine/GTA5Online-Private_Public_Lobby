using System.Collections.Generic;

namespace CodeSwine_Solo_Public_Lobby.Models
{
    public class MWhitelist
    {
        public List<string> Ips { get; set; }
        public bool AllowLanAccess { get; set; }

        public MWhitelist()
        {
            Ips = new List<string>();
        }
    }
}
