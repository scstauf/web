using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace UPSTracker
{
    public class TrackingData
    {
        public DateTime DeliveredOn { get; set; }       //  $('.marginBegin dl dd')[0]
        public string LeftAt { get; set; }              //  $('.marginBegin dl dd')[1]
        public string Status { get; set; }              //  $('#tt_spStatus').text()
        public string Data { get; set; }
        public string TrackingNumber { get; set; }
        public TrackingData(string trackingNumber, string data)
        {
            this.TrackingNumber = trackingNumber;
            this.Data = data;
            this.ParseData();
        }

        public string ToString()
        {
            return string.Format(
                "Tracking #: {0}\r\nStatus: {1}\r\nDelivered On: {2}\r\nLeft At: {3}",
                this.TrackingNumber,
                this.Status,
                this.DeliveredOn,
                this.LeftAt
            );
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("\n", "").Replace("\t", "");
        }

        private void ErrorOut()
        {
            this.DeliveredOn = new DateTime(1900, 1, 1);
            this.Status = "N/A";
            this.LeftAt = "N/A";
        }

        private void ParseData()
        {
            if (string.IsNullOrEmpty(this.Data))
                return;

            try
            {
                if (this.Data.IndexOf("UPS could not locate the shipment") > -1)
                {
                    ErrorOut();
                    return;
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(this.Data);


                if (doc.DocumentNode.SelectNodes("//label[@class='error']") != null)
                    ErrorOut();
                else
                {
                    this.Status = CleanString(doc.GetElementbyId("tt_spStatus").InnerText);

                    var nodes = doc.DocumentNode.SelectNodes("//dl");
                    if (nodes.Count > 3)
                    {
                        var deliveryTokens = CleanString(nodes[2].InnerText)
                            .Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);
                        if (deliveryTokens.Length > 0)
                            this.DeliveredOn = Convert.ToDateTime(deliveryTokens[1].TrimEnd(new char[] { 'a', 't' }));

                        var leftAtTokens = CleanString(nodes[3].InnerText).Split(new char[] { ':' });
                        if (leftAtTokens.Length > 0)
                            this.LeftAt = leftAtTokens[1];
                    }
                }

                Console.WriteLine("Done!");
            }
            catch (Exception e)
            {
                Helpers.ErrorHelper.Log(e);
            }
        }
    }
}
