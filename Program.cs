using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace CRMService
{
    class Program
    {
        //static void Main(string[] args)
        //{

            



        //    // Get the CRM connection string and connect to the CRM Organization
        //    CrmServiceClient crmConn = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString);
        //    IOrganizationService crmService = crmConn.OrganizationServiceProxy;

        //    Entity acc = new Entity("account");
        //    acc["name"] = "Joe's New Account";
        //    crmService.Create(acc);
        //}

        static IOrganizationService _service;
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22");

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            var rt = reader.ReadToEnd();

            JavaScriptSerializer j = new JavaScriptSerializer();
            Rootobject a = (Rootobject)j.Deserialize(rt, typeof(Rootobject));

            Console.WriteLine(rt);


            string name = a.name;
            
            reader.Close();
            response.Close();



            ConnectToMSCRM("arunav@rbhyd12345678.onmicrosoft.com", "Qwerty@123", "https://rbhyd12345678.crm11.dynamics.com/XRMServices/2011/Organization.svc");
            //Guid userid = ((WhoAmIResponse)_service.Execute(new WhoAmIRequest())).UserId;

            //OrganizationRequest req = new OrganizationRequest("new_ReturnOutputParamAsEntityCollection");
            //OrganizationResponse res = _service.Execute(req);

            Entity Account = new Entity("account");
            Account["name"] = name;
            _service.Create(Account);



        }

        public static void ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                _service = (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
