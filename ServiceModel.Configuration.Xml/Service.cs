using System.Collections.Generic;

namespace ServiceModel.Configuration.Xml
{
    public class Service
    {
        public string Name { get; set; }

        public IEnumerable<Endpoint> Endpoints { get; set; }
    }
}
