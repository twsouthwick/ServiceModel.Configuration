using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WcfServer
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var httpUrl = new Uri("http://localhost:8090/roles");
            var host = new ServiceHost(typeof(RoleServiceImpl), httpUrl);

            host.AddServiceEndpoint(typeof(IRoleService), new BasicHttpBinding(), string.Empty);

            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true
            };

            host.Description.Behaviors.Add(smb);

            host.Open();

            Console.WriteLine($"Service is hosted at {httpUrl}");
            Console.WriteLine("Press <Enter> key to stop.");
            Console.ReadLine();
        }
    }
}