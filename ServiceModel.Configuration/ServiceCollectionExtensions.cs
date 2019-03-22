namespace ServiceModel.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void Add<T>(this ServiceModelCollection collection, ServiceModelService service)
        {
            collection.Add(typeof(T), service);
        }

        public static bool TryAdd<T>(this ServiceModelCollection collection, ServiceModelService service)
        {
            return collection.TryAdd(typeof(T), service);
        }
    }
}
