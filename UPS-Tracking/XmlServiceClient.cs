using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Web;

namespace UPSTracker
{
    class XmlServiceClient
    {
        private const string XML_DECLARATION = "<?xml version=\"1.0\"?>\r\n";
        private static string RequestTrackingDetails(string url, string payload)
        {
            var details = new StringBuilder();
            var result = string.Empty;
            WebResponse response = null;
            Stream send = null, recv = null;
            StreamReader reader = null;

            try
            {
                var req = HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                var bytes = Encoding.ASCII.GetBytes(payload.ToString());
                req.ContentLength = bytes.Length;

                using (send = req.GetRequestStream())
                {
                    send.Write(bytes, 0, bytes.Length);
                }

                using (response = req.GetResponse())
                {
                    using (recv = response.GetResponseStream())
                    {
                        using (reader = new StreamReader(recv, Encoding.GetEncoding("utf-8")))
                        {
                            const int BUFFER_MAX = 65536;
                            var buffer = new char[BUFFER_MAX];
                            int count = reader.Read(buffer, 0, BUFFER_MAX);

                            while (count > 0)
                            {
                                details.Append(new string(buffer, 0, BUFFER_MAX));
                                Array.Clear(buffer, 0, BUFFER_MAX);
                                count = reader.Read(buffer, 0, BUFFER_MAX);
                            }

                            details.Append(new string(buffer, 0, BUFFER_MAX));

                            result = details.ToString().Replace(XML_DECLARATION, "");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Helpers.ErrorHelper.Log(e);
            }

            return result;
        }

        public static string RequestTrackingDetails(string trackingNumber)
        {
            Console.WriteLine("Processing Tracking #{0}", trackingNumber);
            var xml = new StringBuilder();
            xml.Append(XML_DECLARATION);
            xml.Append("<AccessRequest xml:lang='en-US'>");
            xml.AppendFormat("<AccessLicenseNumber>{0}</AccessLicenseNumber>", ConfigurationManager.AppSettings["UPSAccessKey"].ToString());
            xml.AppendFormat("<UserId>{0}</UserId>", ConfigurationManager.AppSettings["UPSUserName"].ToString());
            xml.AppendFormat("<Password>{0}</Password>", ConfigurationManager.AppSettings["UPSPassword"].ToString());
            xml.Append("</AccessRequest>");
            xml.Append("<?xml version=\"1.0\"?>");
            xml.Append("<TrackRequest>");
            xml.Append("<Request>");
            xml.Append("<TransactionReference>");
            xml.AppendFormat("<CustomerContext>{0}</CustomerContext>", Guid.NewGuid().ToString());
            xml.Append("</TransactionReference>");
            xml.Append("<RequestAction>Track</RequestAction>");
            xml.Append("</Request>");
            xml.AppendFormat("<TrackingNumber>{0}</TrackingNumber>", trackingNumber);
            xml.Append("</TrackRequest>");
            
            return RequestTrackingDetails("https://www.ups.com/ups.app/xml/Track", xml.ToString());
        }
    }
}
