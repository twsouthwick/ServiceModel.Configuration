using Microsoft.Extensions.Options;

namespace ServiceModel.Configuration
{
    internal class ServiceEndpointValidation : IPostConfigureOptions<ServiceModelOptions>
    {
        public void PostConfigure(string name, ServiceModelOptions options)
        {
        }
    }
}
