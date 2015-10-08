using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPSTracker
{
    class Program
    {
        enum TrackerMethod
        {
            Html,
            Ws,
            Xml
        }

        static string TrimNullBytes(string input)
        {
            var output = new StringBuilder();

            foreach (char c in input.ToCharArray())
                if (c != '\0')
                    output.Append(c);

            return output.ToString();
        }

        static void Main(string[] args)
        {
            //// Extrapolate data from UPS XML responses
            //var outputPath = @"D:\output.txt";
            //var trackingDetails = new List<Helpers.TrackingXmlParser.TrackingDetail>();
            //var parser = new Helpers.TrackingXmlParser();
            //foreach (var xml in System.IO.File.ReadAllLines(outputPath))
            //    trackingDetails.Add(parser.Extrapolate(xml, string.Empty));

            //// Get XML data from UPS
            //var method = TrackerMethod.Xml;
            //var trackingNumbersPath = @"D:\tracking_numbers.txt";
            //var service = new UPSTracker.Helpers.TrackingServiceHelper();
            //var trackingNumbers = System.IO.File.ReadAllLines(trackingNumbersPath);
            //int i = 0, end = trackingNumbers.Length;
            //foreach (var trackingNumber in trackingNumbers)
            //{
            //    i++;
            //    Console.Write("[{0}/{1}]: ", i, end);
            //    var response = string.Empty;
            //    switch (method)
            //    {
            //        case TrackerMethod.Html:
            //            response = UPSTracker.Helpers.UPSTracker.RequestTrackingData(trackingNumber).ToString();
            //            break;
            //        case TrackerMethod.Ws:
            //            response = service.GetDeliveryInformation(trackingNumber);
            //            break;
            //        case TrackerMethod.Xml:
            //            response = XmlServiceClient.RequestTrackingDetails(trackingNumber);
            //            break;
            //    }

            //    System.IO.File.AppendAllText(outputPath, TrimNullBytes(response));
            //}

            //var startDate = DateTime.Now.AddMonths(-1);
            //var endDate = DateTime.Now;
            //var trackingNumbers = Helpers.DataHelper.GetTrackingNumbers(startDate, endDate);

            //foreach (System.Data.DataRow row in trackingNumbers.Rows)
            //{
            //    var trackingNumber = row["TrackingNumber"].ToString();
            //    var trackingData = UPSTracker.Helpers.UPSTracker.RequestTrackingData(trackingNumber);
            //    Console.WriteLine(trackingData.ToString());
            //}

            Console.WriteLine("Done processing!");
            Console.ReadLine();
        }
    }
}
