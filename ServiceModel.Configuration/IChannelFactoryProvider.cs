using System.ServiceModel;

namespace ServiceModel.Configuration
{
    public interface IChannelFactoryProvider<T>
    {
        ChannelFactory<T> CreateChannelFactory(string name);
    }
}
