//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace App_Code.AdvantShop.Payment.AmazonSimplePay
{
    public class AmazonResponseUtils
    {
        private const String SIGNATURE_KEYNAME = "signature";
        private const String SIGNATURE_METHOD_KEYNAME = "signatureMethod";
        private const String SIGNATURE_VERSION_KEYNAME = "signatureVersion";
        private const String SIGNATURE_VERSION_2 = "2";
        private const String CERTIFICATE_URL_KEYNAME = "certificateUrl";

        private const string FPS_PROD_ENDPOINT = "https://fps.amazonaws.com/";
        private const string FPS_SANDBOX_ENDPOINT = "https://fps.sandbox.amazonaws.com/";

        private const string ACTION_PARAM = "?Action=VerifySignature";
        private const string END_POINT_PARAM = "&UrlEndPoint=";
        private const string HTTP_PARAMS_PARAM = "&HttpParameters=";
        // current FPS API version, needs updates if new API versions are released
        private const string VERSION_PARAM_VALUE = "&Version=2008-09-17";

        private const string SUCCESS_RESPONSE =
            "<VerificationStatus>Success</VerificationStatus>";

        protected static readonly string USER_AGENT_STRING = "SigV2_MigrationSampleCode_CS-2010-09-13";

        /**
         * Your aws access key. 
         */

        /**
         * Your 40 character aws secret key. Required only for ipn or return url verification signed using signature version1.
         */
        private readonly String awsSecretKey;

        /**
         *  Use this for verifying IPNs or return urls signed using signature version 2.
         */
        public AmazonResponseUtils()
        {
        }

        /**
         *  Use this for verifying IPNs or return urls signed using signature version 1.
         */
        public AmazonResponseUtils(String awsSecretKey)
        {
            this.awsSecretKey = awsSecretKey;
        }

        /**
         * Validates the request by checking the integrity of its parameters.
         * @param parameters - all the http parameters sent in IPNs or return urls. 
         * @param urlEndPoint should be the url which recieved this request. 
         * @param httpMethod should be either POST (IPNs) or GET (returnUrl redirections)
         */
        public Boolean ValidateRequest(IDictionary<String, String> parameters,
               String urlEndPoint, String httpMethod)
        {
            String signatureVersion;
            //This is present only in case of signature version 2. If this is not present we assume this is signature version 1.
            try
            {
                signatureVersion = parameters[SIGNATURE_VERSION_KEYNAME];
            }
            catch (KeyNotFoundException)
            {
                signatureVersion = null;
            }

            return SIGNATURE_VERSION_2.Equals(signatureVersion) ? ValidateSignatureV2(parameters, urlEndPoint, httpMethod) : ValidateSignatureV1(parameters);
        }

        public bool ValidateSignatureV2(IDictionary<string, string> parameters,
               string urlEndPoint, string httpMethod)
        {
            //1. input validation.
            if (parameters == null || parameters.Keys.Count == 0)
            {
                throw new Exception("must provide http parameters.");
            }

            string signature = parameters[SIGNATURE_KEYNAME];
            if (String.IsNullOrEmpty(signature))
            {
                throw new Exception("'signature' is missing from the parameters.");
            }

            string signatureVersion = parameters[SIGNATURE_VERSION_KEYNAME];
            if (String.IsNullOrEmpty(signatureVersion))
            {
                throw new Exception("'signatureVersion' is missing from the parameters.");
            }

            string signatureMethod = parameters[SIGNATURE_METHOD_KEYNAME];
            if (String.IsNullOrEmpty(signatureMethod))
            {
                throw new Exception("'signatureMethod' is missing from the parameters.");
            }

            string certificateUrl = parameters[CERTIFICATE_URL_KEYNAME];
            if (certificateUrl == null)
            {
                throw new Exception("'certificateUrl' is missing from the parameters.");
            }

            if (String.IsNullOrEmpty(urlEndPoint))
            {
                throw new Exception("must provide url end point.");
            }

            var requestUrlBuilder = new StringBuilder();

            // 2. determine production or sandbox endpoint
            if (certificateUrl.StartsWith(FPS_PROD_ENDPOINT))
            {
                requestUrlBuilder.Append(FPS_PROD_ENDPOINT);
            }
            else if (certificateUrl.StartsWith(FPS_SANDBOX_ENDPOINT))
            {
                requestUrlBuilder.Append(FPS_SANDBOX_ENDPOINT);
            }
            else
            {
                throw new Exception("certificate url was incorrect.");
            }

            // 3. build VerifySignature request URL
            requestUrlBuilder.Append(ACTION_PARAM);
            requestUrlBuilder.Append(END_POINT_PARAM);
            requestUrlBuilder.Append(urlEndPoint);
            requestUrlBuilder.Append(HTTP_PARAMS_PARAM);
            requestUrlBuilder.Append(buildHttpParams(parameters));
            requestUrlBuilder.Append(VERSION_PARAM_VALUE);

            // 4. make call to VerifySignature API
            string verifySignatureResponse = getUrlContents(requestUrlBuilder.ToString());
            return verifySignatureResponse.Contains(SUCCESS_RESPONSE);
        }

        private static string buildHttpParams(IDictionary<string, string> httpParams)
        {
            var paramsBuilder = new StringBuilder();
            Boolean first = true;
            foreach (string key in httpParams.Keys)
            {
                if (!first)
                {
                    paramsBuilder.Append("&");
                }
                else
                {
                    first = false;
                }
                paramsBuilder.Append(urlEncode(key));
                paramsBuilder.Append("=");
                paramsBuilder.Append(urlEncode(httpParams[key]));
            }
            return urlEncode(paramsBuilder.ToString());
        }

        private static string urlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        private static string getUrlContents(string url)
        {
            Console.WriteLine(url);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;
            request.UserAgent = USER_AGENT_STRING;

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception("recieved response error", ex);
            }
        }

        private Boolean ValidateSignatureV1(IDictionary<String, String> parameters)
        {
            if (awsSecretKey == null)
            {
                throw new Exception("Secret key should be set");
            }
            string signature;
            if (!String.IsNullOrEmpty(parameters[SIGNATURE_KEYNAME]))
            {
                signature = parameters[SIGNATURE_KEYNAME];
            }
            else
            {
                throw new Exception("'signature' is missing from the parameters.");
            }

            String sig;
            try
            {
                var encoding = new ASCIIEncoding();
                HMAC hmac = HMAC.Create("HmacSHA1");
                hmac.Key = encoding.GetBytes(awsSecretKey);
                hmac.Initialize();
                var cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
                String stringToSign = CalculateStringToSignV1(parameters);
                cs.Write(encoding.GetBytes(stringToSign), 0, encoding.GetBytes(stringToSign).Length);
                cs.Close();
                byte[] rawResult = hmac.Hash;
                sig = Convert.ToBase64String(rawResult, 0, rawResult.Length);

            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate HMAC : " + e.Message);
            }
            return sig.Equals(signature);
        }

        private static String CalculateStringToSignV1(IDictionary<String, String> parameters)
        {
            var data = new StringBuilder();
            foreach (var pair in new SortedDictionary<String, String>(parameters, StringComparer.OrdinalIgnoreCase).Where(pair => pair.Value != null && String.Compare(pair.Key, SIGNATURE_KEYNAME, true) != 0))
            {
                data.Append(pair.Key);
                data.Append(pair.Value);
            }
            return data.ToString();
        }
    }
}