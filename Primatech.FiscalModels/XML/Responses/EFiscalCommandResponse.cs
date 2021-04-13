using Newtonsoft.Json;
using Primatech.FiscalModels.XML.Base;
using Primatech.FiscalModels.XML.Requests;
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Responses
{

    [Serializable]
    [XmlRoot(ElementName = "FiscalCommandResponse")]
    public class EFCommandResponse<T> : EFCommandResponse where T : class
    {
        [XmlElement(ElementName = "Data")]
        [XmlElement(Type = typeof(EFiscalReceiptCommand))]
        public T Data { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "TCRCommandResponse")]
    public class EFTCRCommandResponse : EFCommandResponse
    {
        public static EFTCRCommandResponse GetFromDerivedClass(EFCommandResponse myBaseClass,string vatNumber,string businessUnitCode,string softwareCode,string tcrCode,string internalIdentifier)
        {
            var newClass = InitWithBaseClass(myBaseClass);
            newClass.ENUInfo = new EFENUInfo()
            {
                BusinessUnitCode = businessUnitCode,
                SoftwareCode = softwareCode,
                ENUCode = tcrCode,
                InternalIdentifier = internalIdentifier,
                VATNumber = vatNumber
            };
            
            return newClass;
        }

        public static EFTCRCommandResponse InitWithBaseClass(EFCommandResponse myBaseClass)
        {
            var newClass =new EFTCRCommandResponse();
            newClass.UIDRequest = myBaseClass.UIDRequest;
            newClass.UIDResponse = myBaseClass.UIDResponse;
            newClass.ResponseCode = myBaseClass.ResponseCode;
            newClass.IsSucccess = myBaseClass.IsSucccess;
            newClass.Error = myBaseClass.Error;
            newClass.ENUIdentifier = myBaseClass.ENUIdentifier;
            
            return newClass;
        }
        [XmlElement(ElementName = "ENUInfo")]
        public EFENUInfo ENUInfo { get; set; }

    }

    public class EFENUInfo
    {
        [XmlElement(ElementName = "VATNumber")]
        public string VATNumber { get; set; }
        [XmlElement(ElementName = "BusinessUnit")]
        public string BusinessUnitCode { get; set; }
        [XmlElement(ElementName = "ENUCode")]
        public string ENUCode { get; set; }
        [XmlElement(ElementName = "SoftwareCode")]
        public string SoftwareCode { get; set; }

        [XmlElement(ElementName = "InternalIdentifier")]
        public string InternalIdentifier { get; set; }
    }

    [XmlRoot(ElementName = "FiscalCommandResponse")]
    public class EFCommandResponse:BaseEFResponse
    {
        public EFCommandResponse() { }

        public EFCommandResponse(string enuIdentifier, Guid uid)
        {
            ENUIdentifier = enuIdentifier;
            UIDRequest = uid.ToString();
        }

        public static EFCommandResponse Create(string printerId)
        {
            var response = new EFCommandResponse();
            response.ENUIdentifier = printerId;
            response.UIDRequest = Guid.NewGuid().ToString();

            return response;
        }

        public static EFCommandResponse CreateSuccess(string printerId)
        {
            var response = Create(printerId);
            response.IsSucccess = true;
            return response;
        }

        /*
        public static EFCommandResponse CreateSuccess(EFiscalReceiptCommand command)
        {
            var response = new EFCommandResponse();
            response.ENUIdentifier = command.ENUIdentifier;
            response.UIDRequest = command.Uid.ToString();
            response.UIDResponse = Guid.NewGuid().ToString();
            var fileName = "[YOUR_INTERNAL_IDENTIFIER]";
            response.Url =new EFUrl(
                "https://efitest.tax.gov.me/ic/#/verify?iic=4B2F2AB8C9AC60BA2F9380A836F62E4E&tin=02399059&crtd=2020-12-06T11:17:31%2001:00&ord=102&bu=zf280xa651&cr=ih694ah861&sw=pf099nu664&prc=228.00");

            response.InternalId = fileName;
            response.IsSucccess = true;
            
            return response;
        }*/

        public static EFCommandResponse CreateError(EFiscalReceiptCommand command, string uidResponse, string code, string message)
        {
           return CreateError(command.ENUIdentifier, command.Uid, uidResponse, code, message);
        }
        public static EFCommandResponse CreateError(string printerId,string uidRequest,string uidResponse,string code, string message)
        {
            var response = new BaseEFCommand()
            {
                PrinterId = printerId,
                UID = uidRequest
            };
            
            return CreateError(response, uidResponse, code, message);
        }

        public static EFCommandResponse CreateError(BaseEFCommand command, string code, string message)
        {
            return CreateError(command, null, code, message);
        }

        public static EFCommandResponse CreateError(BaseEFCommand command,string uidResponse,string code,string message)
        {
            var response = new EFCommandResponse();
            response.ENUIdentifier = command.PrinterId;
            response.UIDRequest = command.UID;
            response.UIDResponse = Guid.NewGuid().ToString();
            response.Error=new EFErrorResponse()
            {
                ErrorCode = code,
                ErrorMessage = message
            };
            return response;
        }

        public static EFCommandResponse CreateSuccess(BaseEFCommand command)
        {
            var response = Create(command);
            response.IsSucccess = true;
            return response;
        }

        public static EFCommandResponse Create(BaseEFCommand command)
        {
            var response = new EFCommandResponse();
            response.ENUIdentifier = command.PrinterId;
            response.UIDRequest = Guid.NewGuid().ToString();
            //var fileName="***"
            //response.InternalId = fileName;
            response.IsSucccess = true;
            return response;
        }


        [XmlElement(ElementName = "Url")]
        public EFUrl Url { get; set; }

        public string ENUIdentifier { get; set; }
        public string UIDRequest { get; set; }
        public string UIDResponse { get; set; }
        public string ResponseCode { get; set; }
        public string InternalId { get; set; }
        //public string TaxPortalUrl { get; set; }
        public string RawMessage { get; set; }
    }

    [XmlRoot(ElementName = "Url")]
    public class EFUrl
    {
        public  EFUrl() { }

        public EFUrl(string url)
        {
            UrlRaw = url;
        }
        [XmlText]
        [JsonIgnore]
        public XmlNode[] CDataContent
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(UrlRaw) };
            }
            set
            {
                if (value == null)
                {
                    UrlRaw = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }

                UrlRaw = value[0].Value;
            }
        }

        [XmlIgnore]
        [JsonProperty("Value")]
        public string UrlRaw { get; set; }

        [XmlElement(ElementName = "UrlContent")]
        public EFUrlContent UrlContent { get; set; }

        public EFUrlContent ToModel()
        {
            var data = UrlRaw.Split('?');
            if (data.Length != 2)
                return null;
            var model = new EFUrlContent()
            {
                BaseUrl = data[0],
                Url= UrlRaw
            };
            var param=data[1].Split('&');
            foreach (var item in param)
            {
                var content=item.Split('=');
                if (content.Length == 2)
                    model.SetKeyAndValue(content[0],content[1]);
            }
            return model;
        }
    }

    [XmlRoot(ElementName = "UrlContent")]
    public class EFUrlContent
    {
        public string BaseUrl { get; set; }
        public string IIC { get; set; }
        public string Tin { get; set; }
        public DateTime CreationDate { get; set; }
        public long DocumentNumber { get; set; }
        public string BusinessUnitCode { get; set; }
        public string TCRCode { get; set; }
        public string SoftwareCode { get; set; }
        public decimal TotalPrice { get; set; }
        public string Url { get; set; }

        public void SetKeyAndValue(string key, string value)
        {

            switch (key)
            {
                case "iic":
                    IIC = value;
                    break;
                case "crtd":
                    DateTime date = DateTime.MinValue;
                    DateTime.TryParseExact(value, new[] {FiscalServiceFormat.DATE_FORMAT_LONG_EU},
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out date);
                    CreationDate = date;
                    break;
                case "tin":
                    Tin = value;
                    break;
                case "ord":
                    long number = 0;
                    Int64.TryParse(value, out number);
                    DocumentNumber = number;
                    break;
                case "bu":
                    BusinessUnitCode = value;
                    break;
                case "cr":
                    TCRCode = value;
                    break;
                case "sw":
                    SoftwareCode = value;
                    break;
                case "prc":
                    var price = 0m;
                    Decimal.TryParse(value, out price);
                    TotalPrice = price;
                    break;
            }

        }
    }
}
