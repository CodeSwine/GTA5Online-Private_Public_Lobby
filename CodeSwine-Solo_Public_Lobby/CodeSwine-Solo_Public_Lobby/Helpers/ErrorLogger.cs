using System;
using System.IO;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class ErrorLogger
    {
        public static void LogException(Exception e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "error.log";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Error log generated at " + DateTime.Now.ToShortDateString());
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(e.Message.ToString());
            }
        }
    }
}