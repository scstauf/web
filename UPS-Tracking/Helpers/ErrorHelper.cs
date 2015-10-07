using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPSTracker.Helpers
{
    public class ErrorHelper
    {
        public static void Log(string message)
        {
            var timestamp = DateTime.Now.ToString();
            var output = string.Format("[{0}]: {1}", timestamp, message);

            Console.WriteLine(output);
        }

        public static void Log(Exception e)
        {
            Log(string.Format("error message: {0}\r\nstack trace: {1}", e.Message, e.StackTrace));
        }
    }
}
