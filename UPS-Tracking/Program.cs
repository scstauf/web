using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPSTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sql = "SELECT TOP 15 TrackingNumber FROM UPSShipments WHERE PlacedDate BETWEEN '20150801' AND '20150901'";

            //foreach (System.Data.DataRow row in Helpers.DataHelper.GetDT(sql).Rows)
            //{
            //    var trackingNumber = row["TrackingNumber"].ToString();
            //    var trackingData = UPSTracker.Helpers.UPSTracker.RequestTrackingData(trackingNumber);
            //    Console.WriteLine(trackingData.ToString());
            //}

            // foreach (var trackingNumber in goodTrackingNumbers)
            // {
                // var trackingData = Helpers.UPSTracker.RequestTrackingData(trackingNumber);
                // Console.WriteLine(trackingData.ToString());
            // }

            // Console.WriteLine("Done processing!");
            // Console.ReadLine();
        }
    }
}
