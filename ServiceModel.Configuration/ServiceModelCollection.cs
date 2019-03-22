using System;
using System.Collections;
using System.Collections.Generic;

namespace ServiceModel.Configuration
{
    public class ServiceModelCollection : IReadOnlyCollection<ServiceModelService>
    {
        private readonly Dictionary<Type, ServiceModelService> _services = new Dictionary<Type, ServiceModelService>();

        public void Add<T>(Action<ServiceModelService> configure) => Add(typeof(T), configure);

        public void Add(Type type, Action<ServiceModelService> configure)
        {
            Validate(type);

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (_services.TryGetValue(type, out var current))
            {
                configure(current);
            }
            else
            {
                var service = new ServiceModelService();

                configure(service);

                _services.Add(type, service);
            }
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
