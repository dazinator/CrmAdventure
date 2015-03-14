using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrmAdventure.OAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthAdventure
    {

        [TestCase(Config.Crm_2013_Online_Organisation_Service_Url)]
        public void Can_Discover_Authority(string orgServiceUrl)
        {
            var sdkClientVersion = typeof(OrganizationServiceProxy).Assembly.GetName().Version;
            var soapEndpointHelper = new SoapEndpointHelper(orgServiceUrl, sdkClientVersion);
            var authority = soapEndpointHelper.GetAuthority();           
            Console.WriteLine(authority);
        }     


        [TestCase(Config.Crm_2013_Online_Organisation_Service_Url,
                  Config.Crm_2013_Online_Org_Url,
                  Config.ClientID,
                  Config.Username)]
        public void Can_Authenticate_Against_Resource(string orgServiceUrl, string resource, string clientId, string userName)
        {

            var sdkClientVersion = typeof(OrganizationServiceProxy).Assembly.GetName().Version;
            var soapEndpointHelper = new SoapEndpointHelper(orgServiceUrl, sdkClientVersion);                      


            var pass = Config.GetPassword();
            var credentials = new UserCredential(userName, pass);
            var authResult = soapEndpointHelper.AuthenticateAsync(resource, clientId, credentials);
            var result = authResult.Result;
            if(result != null)
            {
                Console.Write(result.AccessTokenType);
                Console.Write(result.AccessToken);
            }

        }            



    }

    public class SoapEndpointHelper
    {
       
        private Uri _Authority = null;
        //private string _Resource = null;   

        public SoapEndpointHelper(string orgServiceUrl, Version clientVersion)
        {
            if (clientVersion == null)
            {
                throw new ArgumentNullException("clientVersion");
            }

            OrgServiceUrl = orgServiceUrl;
            ClientVersion = clientVersion;

            string endpointUrl = string.Format("{0}/web?SdkClientVersion={1}", orgServiceUrl, clientVersion.ToString());
            SoapEndpoint = new Uri(endpointUrl);
        }

        public Uri GetAuthority()
        {
            if (_Authority == null)
            {
                _Authority = DiscoveryAuthority();               
            }           
            return _Authority;
        }

        /// <summary>
        /// Discover the authority for authentication.
        /// </summary>
        /// <param name="serviceUrl">The SOAP endpoint for a tenant organization.</param>
        /// <returns>The decoded authority URL.</returns>
        /// <remarks>The passed service URL string must contain the SdkClientVersion property.
        /// Otherwise, the discovery feature will not be available.</remarks>
        private Uri DiscoveryAuthority()
        {          
            // Use AuthenticationParameters to send a request to the organization's endpoint and
            // receive tenant information in the 401 challenge. 
            AuthenticationParameters parameters = null;

            HttpWebResponse response = null;
            try
            {


                // Create a web request where the authorization header contains the word "Bearer".
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(SoapEndpoint);
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization.ToString(), "Bearer");

                // The response is to be encoded.
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                response = (HttpWebResponse)httpWebRequest.GetResponse();

                // If the expected response is returned, this code should not execute.
                throw new AdalException("unauthorized_response_expected", "Unauthorized http response (status code 401) was expected");
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (response == null)
                {
                    throw new AdalException("Unauthorized Http Status Code (401) was expected in the response", (Exception)ex);
                }
                else
                {

                    // A good response was returned. Extract any parameters from the response.
                    // The response should contain an authorization_uri parameter.
                    parameters = AuthenticationParameters.CreateFromUnauthorizedResponse(response);
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            // Return the authority URL.
           // _Resource = parameters.Resource;
            Uri authorityUri = new Uri(parameters.Authority);
            return authorityUri;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string resource, string clientId, UserCredential credential)
        {
            var auth = GetAuthority();
            // Obtain an authentication token to access the web service.
            var authenticationContext = new AuthenticationContext(auth.ToString(), false);            
            AuthenticationResult result = await authenticationContext.AcquireTokenAsync(resource, clientId, credential);
            return result;
        }

        public string OrgServiceUrl { get; set; }
        public Uri SoapEndpoint { get; set; }
        public Version ClientVersion { get; set; }



    }
}
