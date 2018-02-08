using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ConsumingExternalWebApi
{
    public class ConsumingExternalWebApi : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            WebRequest request = WebRequest.Create("http://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22");

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            var rt = reader.ReadToEnd();
            Rootobject a = Deserialise<Rootobject>(rt);

            Console.WriteLine(rt);


            string name = a.name;

            reader.Close();
            response.Close();
            

            Entity Account = new Entity("account");
            Account["name"] = name;
            service.Create(Account);
        }
        public T Deserialise<T>(string json)
        {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                T result = (T)deserializer.ReadObject(stream);
                return result;
            }
        }
    }
}
