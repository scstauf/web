using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace UPSTracker.Helpers
{
    public class TrackingXmlParser
    {
        private const string XML_DECLARATION = "<?xml version=\"1.0\"?>";

        private List<string> properties = new List<string>();
        public List<string> Properties
        {
            get
            {
                return this.properties;
            }
            set
            {
                this.properties = value;
            }
        }

        private void BuildTrackingDetail(ref TrackingDetail trackingDetail, string propertyName, string value)
        {
            properties.Add(propertyName);
            switch (propertyName)
            {
                case "ResponseResponseStatusCode":
                    trackingDetail.ResponseStatusCode = value;
                    break;
                case "ResponseStatusDescription":
                    trackingDetail.ResponseStatusDescription = value;
                    break;
                case "ShipmentShipperShipperNumber":
                    trackingDetail.ShipperNumber = value;
                    break;
                case "ShipperAddressAddressLine1":
                    trackingDetail.ShipperAddressLine1 = value;
                    break;
                case "AddressCity":
                    trackingDetail.ShipperAddressCity = value;
                    break;
                case "AddressStateProvinceCode":
                    trackingDetail.ShipperAddressState = value;
                    break;
                case "PostalCode":
                    trackingDetail.ShipperAddressZip = value;
                    break;
                case "CountryCode":
                    if (string.IsNullOrEmpty(trackingDetail.ShipperAddressCountry))
                        trackingDetail.ShipperAddressCountry = value;
                    else if (string.IsNullOrEmpty(trackingDetail.ShipToAddressCountry))
                        trackingDetail.ShipToAddressCountry = value;
                    else if (string.IsNullOrEmpty(trackingDetail.ActivityAddressCountry))
                        trackingDetail.ActivityAddressCountry = value;
                    break;
                case "ShipToAddressCity":
                    trackingDetail.ShipToAddressCity = value;
                    break;
                case "ShipToAddressStateProvinceCode":
                    trackingDetail.ShipToAddressState = value;
                    break;
                case "ShipmentWeightUnitOfMeasurementCode":
                    trackingDetail.ShipmentWeightUnitOfMeasurement = value;
	                break;
                case "ShipmentWeightWeight":
                    trackingDetail.ShipmentWeight = value;
	                break;
                case "ServiceCode":
                    trackingDetail.ServiceCode = value;
	                break;
                case "ServiceDescription":
                    trackingDetail.ServiceDescription = value;
                    break;
                case "ShipmentIdentificationNumber":
                    trackingDetail.ShipmentIdentificationNumber = value;
	                break;
                case "PickupDate":
                    trackingDetail.PickupDate = value;
	                break;
                case "DeliveryDateUnavailableType":
                    trackingDetail.DeliveryDateUnavailableType = value;
	                break;
                case "DeliveryDateUnavailableDescription":
                    trackingDetail.DeliveryDateUnavailableDescription = value;
                    break;
                case "PackageTrackingNumber":
                    trackingDetail.PackageTrackingNumber = value;
	                break;
                case "ActivityActivityLocationAddressCity":
                    trackingDetail.ActivityAddressCity = value;
                    break;
                case "ActivityLocationAddressStateProvinceCode":
                    trackingDetail.ActivityAddressState = value;
                    break;
                case "AddressPostalCode":
                    if (string.IsNullOrEmpty(trackingDetail.ShipToAddressZip))
                        trackingDetail.ShipToAddressZip = value;
                    else if (string.IsNullOrEmpty(trackingDetail.ActivityAddressZip))
                        trackingDetail.ActivityAddressZip = value;
                    break;
                case "Code":
                    trackingDetail.ActivityLocationCode = value;
                    break;
                case "Description":
                    trackingDetail.ActivityLocationDescription = value;
                    break;
                case "SignedForByName":
                    trackingDetail.SignedForByName = value;
	                break;
                case "StatusStatusTypeCode":
                    trackingDetail.StatusTypeCode = value;
	                break;
                case "StatusStatusTypeDescription":
                    trackingDetail.StatusTypeDescription = value;
                    break;
                case "StatusCodeCode":
                    trackingDetail.StatusCode = value;
	                break;
                case "Date":
                    trackingDetail.Date = value;
	                break;
                case "Time":
                    trackingDetail.Time = value;
	                break;
                case "PackageWeightUnitOfMeasurementCode":
                    trackingDetail.PackageWeightUnitOfMeasurement = value;
                    break;
                case "PackageWeightWeight":
                    trackingDetail.PackageWeight = value;
                    break;
            }
        }

        public TrackingDetail Extrapolate(string xml, string trackingNumber)
        {
            if (xml.Equals(XML_DECLARATION))
                return null;
            if (xml.Contains(XML_DECLARATION))
                xml = xml.Replace(XML_DECLARATION, "");

            var trackingDetail = new TrackingDetail();
            var parseMe = new StringBuilder();
            parseMe.Clear();
            parseMe.Append(XML_DECLARATION);
            parseMe.Append(xml);

            try
            {
                using (var reader = XmlReader.Create(new StringReader(parseMe.ToString())))
                {
                    var greatGrandParent = string.Empty;
                    var grandParent = string.Empty;
                    var parent = string.Empty;
                    var name = string.Empty;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (!name.Equals(reader.Name))
                                {
                                    greatGrandParent = grandParent;
                                    grandParent = parent;
                                    parent = name;
                                    name = reader.Name;
                                }
                                break;
                            case XmlNodeType.Text:
                                BuildTrackingDetail(
                                    ref trackingDetail, 
                                    string.Format("{0}{1}{2}{3}", greatGrandParent, grandParent, parent, name), 
                                    reader.Value
                                );
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name.Equals(greatGrandParent))
                                    greatGrandParent = string.Empty;
                                if (reader.Name.Equals(grandParent))
                                    grandParent = string.Empty;
                                if (reader.Name.Equals(parent))
                                    parent = string.Empty;
                                name = string.Empty;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Log(e);
            }

            this.properties.Clear();

            return trackingDetail;
        }

        public class TrackingDetail
        {
            public string ResponseStatusCode { get; set; }
            public string ResponseStatusDescription { get; set; }
            public string ShipperNumber { get; set; }
            public string ShipperAddressLine1 { get; set; }
            public string ShipperAddressCity { get; set; }
            public string ShipperAddressState { get; set; }
            public string ShipperAddressZip { get; set; }
            public string ShipperAddressCountry { get; set; }
            public string ShipToAddressCity { get; set; }
            public string ShipToAddressState { get; set; }
            public string ShipToAddressZip { get; set; }
            public string ShipToAddressCountry { get; set; }
            public string ShipmentWeightUnitOfMeasurement { get; set; }
            public string ShipmentWeight { get; set; }
            public string ServiceCode { get; set; }
            public string ServiceDescription { get; set; }
            public string ShipmentIdentificationNumber { get; set; }
            public string PickupDate { get; set; }
            public string DeliveryDateUnavailableType { get; set; }
            public string DeliveryDateUnavailableDescription { get; set; }
            public string PackageTrackingNumber { get; set; }
            public string ActivityAddressCity { get; set; }
            public string ActivityAddressState { get; set; }
            public string ActivityAddressZip { get; set; }
            public string ActivityAddressCountry { get; set; }
            public string ActivityLocationCode { get; set; }
            public string ActivityLocationDescription { get; set; }
            public string SignedForByName { get; set; }
            public string StatusTypeCode { get; set; }
            public string StatusTypeDescription { get; set; }
            public string StatusCode { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string PackageWeightUnitOfMeasurement { get; set; }
            public string PackageWeight { get; set; }
        }
    }
}
