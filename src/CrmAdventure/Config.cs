using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CrmAdventure
{
    public static class Config
    {
        public const string ClientID = "893262be-fbdc-4556-9325-9f863b69495b";
        public const string Crm_2013_Online_Organisation_Service_Url = "https://crmadotrial4.api.crm4.dynamics.com/XRMServices/2011/Organization.svc";
        public const string Crm_2013_Online_Org_Url = "https://crmadotrial4.api.crm4.dynamics.com";
        public const string Username = "testing@crmadotrial4.onmicrosoft.com";

        public static string GetPassword()
        {
            var password = ConfigurationManager.AppSettings["Password"];
            return password;
        }


    }
}
