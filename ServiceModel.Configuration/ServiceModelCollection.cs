using System;
using System.Collections;
using System.Collections.Generic;

namespace ServiceModel.Configuration
{
    public class ServiceModelCollection : IReadOnlyCollection<ServiceModelService>
    {
        private readonly Dictionary<Type, ServiceModelService> _services = new Dictionary<Type, ServiceModelService>();

        public void Add(Type type, ServiceModelService service)
        {
            Validate(type);

            _services[type] = service ?? throw new ArgumentNullException(nameof(service));
        }

        public bool TryAdd(Type type, ServiceModelService service)
        {
            Validate(type);

            if (!_services.ContainsKey(type))
            {
                _services[type] = service ?? throw new ArgumentNullException(nameof(service));
                return true;
            }

            return false;
        }

        public bool TryGet<T>(out ServiceModelService service) => _services.TryGetValue(typeof(T), out service);

        public int Count => _services.Count;

        public IEnumerator<ServiceModelService> GetEnumerator() => _services.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static void Validate(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsInterface)
            {
                throw new ServiceConfigurationException("Type must be an interface");
            }
        }
    }
}
