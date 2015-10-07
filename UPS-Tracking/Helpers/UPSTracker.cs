using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace UPSTracker.Helpers
{
    public class UPSTracker
    {
        public const string URL = "http://wwwapps.ups.com/WebTracking/track?loc=en_US&tbifl=1&hiddenText=&tracknum={0}&track.x=Track&trackSelectedOption=";

        private static string GetRequestURL(string trackingNumber)
        {
            return string.Format(URL, trackingNumber);
        }

        private static string RequestHtml(string url)
        {
            var responseString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    responseString = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Log(e);
            }

            return responseString;
        }

        public static TrackingData RequestTrackingData(string trackingNumber)
        {
            var requestUrl = GetRequestURL(trackingNumber);
            var responseData = RequestHtml(requestUrl);

            if (string.IsNullOrEmpty(responseData))
            {
                ErrorHelper.Log(string.Format("Response empty for requested url: {0}", requestUrl));
                return null;
            }

            var trackingData = new TrackingData(trackingNumber, responseData);

            return trackingData;
        }
    }
}
