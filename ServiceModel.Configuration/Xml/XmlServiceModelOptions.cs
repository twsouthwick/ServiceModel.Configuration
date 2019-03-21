using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Xml;

namespace ServiceModel.Configuration.Xml
{
    internal class XmlServiceModelOptions : IConfigureNamedOptions<ServiceModelOptions>
    {
        public XmlServiceModelOptions(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                Configuration = ServiceModelConfiguration.Parse(reader);
            }
        }

        public XmlServiceModelOptions(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                Configuration = ServiceModelConfiguration.Parse(reader);
            }
        }

        public XmlServiceModelOptions(TextReader text)
        {
            using (var reader = XmlReader.Create(text))
            {
                Configuration = ServiceModelConfiguration.Parse(reader);
            }
        }

        public ServiceModelConfiguration Configuration { get; }

        public void Configure(string name, ServiceModelOptions options) => throw new NotImplementedException();

        public void Configure(ServiceModelOptions options) => throw new NotImplementedException();
    }
}
