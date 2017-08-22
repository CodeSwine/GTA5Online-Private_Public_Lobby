using CodeSwine_Solo_Public_Lobby.Helpers;
using CodeSwine_Solo_Public_Lobby.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CodeSwine_Solo_Public_Lobby.DataAccess
{
    public class DaWhitelist
    {
        public IPTool iPTool = new IPTool();
        private bool useWhitelist;
        private string iPCount;

        public DaWhitelist()
        {
            ReadIPsFromJSON();
        }

        public List<IPAddress> IpAddressess {
            get { return ReadIPsFromJSON(); }
        }

        public string IPCount {
            get { return iPCount; }
        }

        public bool UseWhitelist {
            get { return useWhitelist; }
            set { useWhitelist = value; }
        }

        List<IPAddress> ReadIPsFromJSON()
        {
            List<IPAddress> addresses = new List<IPAddress>();
            
            string path = Environment.ExpandEnvironmentVariables(AppDomain.CurrentDomain.BaseDirectory + "settings.json");

            string json = "";

            if(!File.Exists(path))
            {
                SaveToJson(new MWhitelist());
            }

            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }

            MWhitelist whitelist = JsonConvert.DeserializeObject<MWhitelist>(json);

            foreach (string address in whitelist.Ips)
            {
                if(iPTool.ValidateIPv4(address.ToString())) addresses.Add(IPAddress.Parse(address));
            }

            iPCount = addresses.Count.ToString();
            return addresses;
        }

        public static void SaveToJson(MWhitelist whitelist)
        {
            string json = JsonConvert.SerializeObject(whitelist);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", json);
        }
    }
}
