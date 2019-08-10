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
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "settings.json";

        public static MWhitelist ReadFromJSON()
        {
            if(!File.Exists(path))
            {
                SaveToJson(new MWhitelist());
            }

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<MWhitelist>(json);
            }
        }

        public static void SaveToJson(MWhitelist whitelist)
        {
            string json = JsonConvert.SerializeObject(whitelist);
            File.WriteAllText(path, json);
        }
    }
}
