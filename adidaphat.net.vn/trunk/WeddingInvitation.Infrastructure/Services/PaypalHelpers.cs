using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace WeddingInvitation.Infrastructure.Services
{
    /// <summary>
    /// Summary description for NVPAPICaller
    /// </summary>
    public class PaypalHelpers
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(NVPAPICaller));

        private string _pendpointurl = "https://api-3t.paypal.com/nvp";
        private const string Cvv2 = "CVV2";

        //Flag that determines the PayPal environment (live or sandbox)
        private static readonly bool UseSandbox = bool.Parse(ConfigurationManager.AppSettings["UseSandbox"].ToString(CultureInfo.InvariantCulture));
        private static readonly string CurrentCode = ConfigurationManager.AppSettings["CurrencyCode"];
        private static readonly string ReturnURL = ConfigurationManager.AppSettings["ReturnURL"];
        private static readonly string CancelURL = ConfigurationManager.AppSettings["CancelURL"];

        private const string Signature = "SIGNATURE";
        private const string Pwd = "PWD";
        private const string Acct = "ACCT";

        //Replace <API_USERNAME> with your API Username
        //Replace <API_PASSWORD> with your API Password
        //Replace <API_SIGNATURE> with your Signature
        private string _apiUsername = ConfigurationManager.AppSettings["ApiUsername"];// "seller_1336483755_biz_api1.gmail.com";
        private string _apiPassword = ConfigurationManager.AppSettings["ApiPassword"]; //"1336483781";
        private string _apiSignature = ConfigurationManager.AppSettings["ApiSignature"]; //"AFcWxV21C7fd0v3bYYYRCpSSRl31AovYz.jPBCcPdC-jl8yEEP9V4Qwv";
        private const string Subject = "";
        private const string BnCode = "PP-ECWizard";

        //HttpWebRequest Timeout specified in milliseconds 
        private const int Timeout = 50000;
        private static readonly string[] SecuredNvps = new string[] { Acct, Cvv2, Signature, Pwd };


        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public void SetCredentials(string userId, string password, string signature)
        {
            _apiUsername = userId;
            _apiPassword = password;
            _apiSignature = signature;
        }

        /// <summary>
        /// ShortcutExpressCheckout: The method that calls SetExpressCheckout API
        /// </summary>
        /// <param name="amount"></param>
        /// <param ref="" name="token"></param>
        /// <param name="retMsg"> <ref></ref> </param>
        /// <returns></returns>
        public bool ShortcutExpressCheckout(string amount, ref string token, ref string retMsg)
        {
            var host = "www.paypal.com";
            if (UseSandbox)
            {
                _pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
                host = "www.sandbox.paypal.com";
            }

            //const string returnURL = "http://localhost:12458/Account/ConfirmPaypal";
            //const string cancelURL = "http://localhost:12458/Account/CancelFromPaypal";

            var encoder = new NvpCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = ReturnURL;
            encoder["CANCELURL"] = CancelURL;
            encoder["AMT"] = amount;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["CURRENCYCODE"] = CurrentCode;

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if ((strAck == "success" || strAck == "successwithwarning"))
            {
                token = decoder["TOKEN"];

                var ecurl = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

                retMsg = ecurl;
                return true;
            }
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                     "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                     "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }

        /// <summary>
        /// MarkExpressCheckout: The method that calls SetExpressCheckout API, invoked from the 
        /// Billing Page EC placement
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="shipToCountryCode"> </param>
        /// <param name="token"> <ref></ref> </param>
        /// <param ref name="retMsg"></param>
        /// <param name="shipToName"> </param>
        /// <param name="shipToStreet"> </param>
        /// <param name="shipToStreet2"> </param>
        /// <param name="shipToCity"> </param>
        /// <param name="shipToState"> </param>
        /// <param name="shipToZip"> </param>
        /// <returns></returns>
        public bool MarkExpressCheckout(string amount,
                            string shipToName, string shipToStreet, string shipToStreet2,
                            string shipToCity, string shipToState, string shipToZip,
                            string shipToCountryCode, ref string token, ref PaypalErrorModel retMsg)
        {
            string host = "www.paypal.com";
            if (UseSandbox)
            {
                _pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
                host = "www.sandbox.paypal.com";
            }

            //const string returnURL = "http://localhost:12458/Account/ConfirmPaypal";
            //const string cancelURL = "http://localhost:12458/Account/CancelFromPaypal";

            var encoder = new NvpCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = ReturnURL;
            encoder["CANCELURL"] = CancelURL;
            encoder["AMT"] = amount;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["CURRENCYCODE"] = CurrentCode;

            //Optional Shipping Address entered on the merchant site
            encoder["SHIPTONAME"] = shipToName;
            encoder["SHIPTOSTREET"] = shipToStreet;
            encoder["SHIPTOSTREET2"] = shipToStreet2;
            encoder["SHIPTOCITY"] = shipToCity;
            encoder["SHIPTOSTATE"] = shipToState;
            encoder["SHIPTOZIP"] = shipToZip;
            encoder["SHIPTOCOUNTRYCODE"] = shipToCountryCode;


            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if ((strAck == "success" || strAck == "successwithwarning"))
            {
                token = decoder["TOKEN"];

                string ecurl = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

                //retMsg = ECURL;
                retMsg = new PaypalErrorModel
                {
                    ErrorCode = "200",
                    Desc = ecurl,
                };
                return true;
            }
            retMsg = new PaypalErrorModel
                         {
                             ErrorCode = decoder["L_ERRORCODE0"],
                             Desc = decoder["L_SHORTMESSAGE0"],
                             Desc2 = decoder["L_LONGMESSAGE0"]
                         };
            //retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
            //    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
            //    "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }


        /// <summary>
        /// GetShippingDetails: The method that calls SetExpressCheckout API, invoked from the 
        /// Billing Page EC placement
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shippingAddress"> </param>
        /// <param ref="" name="retMsg"></param>
        /// <returns></returns>
        public bool GetShippingDetails(string token, ref string payerId, ref ShippingAddress shippingAddress, ref PaypalErrorModel retMsg)
        {

            if (UseSandbox)
            {
                _pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            }

            var encoder = new NvpCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            var pStrrequestforNvp = encoder.Encode();
            var pStresponsenvp = HttpCall(pStrrequestforNvp);

            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            payerId = decoder["PAYERID"].ToLower();
            if ((strAck == "success" || strAck == "successwithwarning"))
            {
                shippingAddress.FirstName = decoder["FIRSTNAME"];
                shippingAddress.LastName = decoder["LASTNAME"];
                shippingAddress.ShipToName = decoder["SHIPTONAME"];
                shippingAddress.Street1 = decoder["SHIPTOSTREET"];
                shippingAddress.Street2 = decoder["SHIPTOSTREET2"];
                shippingAddress.City = decoder["SHIPTOCITY"];
                shippingAddress.State = decoder["SHIPTOSTATE"];
                shippingAddress.Zip = decoder["SHIPTOZIP"];

                //ShippingAddress = "<table><tr>";
                //ShippingAddress += "<td> First Name </td><td>" + decoder["FIRSTNAME"] + "</td></tr>";
                //ShippingAddress += "<td> Last Name </td><td>" + decoder["LASTNAME"] + "</td></tr>";
                //ShippingAddress += "<td colspan='2'> Address</td></tr>";
                //ShippingAddress += "<td> Name </td><td>" + decoder["SHIPTONAME"] + "</td></tr>";
                //ShippingAddress += "<td> Street1 </td><td>" + decoder["SHIPTOSTREET"] + "</td></tr>";
                //ShippingAddress += "<td> Street2 </td><td>" + decoder["SHIPTOSTREET2"] + "</td></tr>";
                //ShippingAddress += "<td> City </td><td>" + decoder["SHIPTOCITY"] + "</td></tr>";
                //ShippingAddress += "<td> State </td><td>" + decoder["SHIPTOSTATE"] + "</td></tr>";
                //ShippingAddress += "<td> Zip </td><td>" + decoder["SHIPTOZIP"] + "</td>";
                //ShippingAddress += "</tr>";

                return true;
            }
            retMsg = new PaypalErrorModel
                         {
                             ErrorCode = decoder["L_ERRORCODE0"],
                             Desc = decoder["L_SHORTMESSAGE0"],
                             Desc2 = decoder["L_LONGMESSAGE0"]
                         };
            //retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
            //    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
            //    "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }


        /// <summary>
        /// ConfirmPayment: The method that calls SetExpressCheckout API, invoked from the 
        /// Billing Page EC placement
        /// </summary>
        /// <param name="finalPaymentAmount"> </param>
        /// <param name="token"></param>
        /// <param name="retMsg"> <ref></ref> </param>
        /// <param name="payerId"> </param>
        /// <returns></returns>
        public bool ConfirmPayment(string finalPaymentAmount, string token, string payerId, ref NvpCodec decoder, ref PaypalErrorModel retMsg)
        {
            if (UseSandbox)
            {
                _pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            }

            var encoder = new NvpCodec();
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["TOKEN"] = token;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["PAYERID"] = payerId;
            encoder["AMT"] = finalPaymentAmount;
            encoder["CURRENCYCODE"] = CurrentCode;

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if ((strAck == "success" || strAck == "successwithwarning"))
            {
                return true;
            }
            retMsg = new PaypalErrorModel
                         {
                             ErrorCode = decoder["L_ERRORCODE0"],
                             Desc = decoder["L_SHORTMESSAGE0"],
                             Desc2 = decoder["L_LONGMESSAGE0"]
                         };
            //retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
            //    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
            //    "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }


        /// <summary>
        /// HttpCall: The main method that is used for all API calls
        /// </summary>
        /// <param name="nvpRequest"></param>
        /// <returns></returns>
        public string HttpCall(string nvpRequest) //CallNvpServer
        {
            string url = _pendpointurl;

            //To Add the credentials from the profile
            string strPost = nvpRequest + "&" + BuildCredentialsNvpString();
            strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode(BnCode);

            var objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Timeout = Timeout;
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;

            try
            {
                using (var myWriter = new StreamWriter(objRequest.GetRequestStream()))
                {
                    myWriter.Write(strPost);
                }
            }
            catch (Exception e)
            {
                /*
                if (log.IsFatalEnabled)
                {
                    log.Fatal(e.Message, this);
                }*/
            }

            //Retrieve the Response returned from the NVP API call to PayPal
            var objResponse = (HttpWebResponse)objRequest.GetResponse();
            string result;
            using (var sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            //Logging the response of the transaction
            /* if (log.IsInfoEnabled)
             {
                 log.Info("Result :" +
                           " Elapsed Time : " + (DateTime.Now - startDate).Milliseconds + " ms" +
                          result);
             }
             */
            return result;
        }

        /// <summary>
        /// Credentials added to the NVP string
        /// </summary>
        /// <returns></returns>
        private string BuildCredentialsNvpString()
        {
            var codec = new NvpCodec();

            if (!IsEmpty(_apiUsername))
                codec["USER"] = _apiUsername;

            if (!IsEmpty(_apiPassword))
                codec[Pwd] = _apiPassword;

            if (!IsEmpty(_apiSignature))
                codec[Signature] = _apiSignature;

            if (!IsEmpty(Subject))
                codec["SUBJECT"] = Subject;

            codec["VERSION"] = "2.3";

            return codec.Encode();
        }

        /// <summary>
        /// Returns if a string is empty or null
        /// </summary>
        /// <param name="s">the string</param>
        /// <returns>true if the string is not null and is not empty or just whitespace</returns>
        public static bool IsEmpty(string s)
        {
            return s == null || s.Trim() == string.Empty;
        }
    }


    public sealed class NvpCodec : NameValueCollection
    {
        private const string AMPERSAND = "&";
        private const string EQUALS = "=";
        private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
        private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

        /// <summary>
        /// Returns the built NVP string of all name/value pairs in the Hashtable
        /// </summary>
        /// <returns></returns>
        public string Encode()
        {
            var sb = new StringBuilder();
            bool firstPair = true;
            foreach (string kv in AllKeys)
            {
                string name = HttpUtility.UrlEncode(kv);
                string value = HttpUtility.UrlEncode(this[kv]);
                if (!firstPair)
                {
                    sb.Append(AMPERSAND);
                }
                sb.Append(name).Append(EQUALS).Append(value);
                firstPair = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Decoding the string
        /// </summary>
        /// <param name="nvpstring"></param>
        public void Decode(string nvpstring)
        {
            Clear();
            foreach (var nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
            {
                string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
                if (tokens.Length >= 2)
                {
                    string name = HttpUtility.UrlDecode(tokens[0]);
                    string value = HttpUtility.UrlDecode(tokens[1]);
                    Add(name, value);
                }
            }
        }


        #region Array methods
        public void Add(string name, string value, int index)
        {
            Add(GetArrayName(index, name), value);
        }

        public void Remove(string arrayName, int index)
        {
            Remove(GetArrayName(index, arrayName));
        }

        /// <summary>
        /// 
        /// </summary>
        public string this[string name, int index]
        {
            get
            {
                return this[GetArrayName(index, name)];
            }
            set
            {
                this[GetArrayName(index, name)] = value;
            }
        }

        private static string GetArrayName(int index, string name)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
            }
            return name + index;
        }
        #endregion
    }

}