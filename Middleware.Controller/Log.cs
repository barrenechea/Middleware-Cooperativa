using System;
using System.IO;

namespace Middleware.Controller
{
    public static class Log
    {
        public static void Add(string logMessage)
        {
            using (var w = File.AppendText("log.txt"))
            {
                w.Write($"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}]");
                w.WriteLine($": {logMessage}");
            }
        }
    }
}