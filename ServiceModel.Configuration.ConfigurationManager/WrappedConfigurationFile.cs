using System;
using System.IO;
using System.ServiceModel.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace ServiceModel.Configuration
{
    internal sealed class WrappedConfigurationFile : IDisposable
    {
        public WrappedConfigurationFile(string path)
        {
            ConfigPath = Path.GetTempFileName();

            var original = XDocument.Load(path);
            WritePath(ConfigPath, original);
        }

        private void WritePath(string configPath, XDocument original)
        {
            var serviceModel = original.Descendants("system.serviceModel");

            var doc = new XDocument(
                new XElement("configuration",
                    new XElement("configSections",
                        new XElement("sectionGroup", new XAttribute("name", "system.serviceModel"), new XAttribute("type", typeof(ServiceModelSectionGroup).AssemblyQualifiedName),
                            new XElement("section", new XAttribute("name", "services"), new XAttribute("type", typeof(ServicesSection).AssemblyQualifiedName)))),
                    serviceModel));

            using (var writer = XmlWriter.Create(configPath, new XmlWriterSettings { Indent = true }))
            {
                doc.WriteTo(writer);
            }
        }

        public string ConfigPath { get; }

        public void Dispose() => File.Delete(ConfigPath);
    }
}
