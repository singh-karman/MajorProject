using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Identity.Client;

namespace Completist
{
    public partial class App : Application
    {
        static App()
        {
            _clientApp = PublicClientApplicationBuilder.Create(ClientID)
                        .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                        .WithDefaultRedirectUri()
                        .Build();
        }
        private static IPublicClientApplication _clientApp;

        private static string ClientID = "5c25068d-b27f-47b9-9472-e77a019d9a48";

        private static string Tenant = "common";
        //private static string ClientSecret = "AqfHU2P4Ia7Csl2e8JTsMfb--~Q.mJ0t23";
        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }



    }
}
