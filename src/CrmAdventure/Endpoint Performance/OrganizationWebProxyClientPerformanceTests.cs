using Microsoft.Xrm.Sdk.WebServiceClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmAdventure
{
    [TestFixture]
    public class OrganizationWebProxyClientPerformanceTests
    {

        public OrganizationWebProxyClientPerformanceTests()
        {


        }

        [Description("Tests how much bandwidth is consumed when a large query is performed.")]
        [Test]
        public void Test_Bandwidth_Consumption_For_Large_Query()
        {

            string orgServiceUrl = GetOrgServiceUrl();
            TimeSpan timeout = new TimeSpan(0, 3, 0);
            Uri endpointUri = new Uri(orgServiceUrl);
            var sdkClientVersion = typeof(OrganizationWebProxyClient).Assembly.GetName().Version.ToString();

            using (OrganizationWebProxyClient client = new OrganizationWebProxyClient(endpointUri, timeout, false))
            {
                client.HeaderToken = "no idea";
                client.SdkClientVersion = sdkClientVersion;




            }



        }

        private string GetOrgServiceUrl()
        {
            throw new NotImplementedException();
        }



    }
}
