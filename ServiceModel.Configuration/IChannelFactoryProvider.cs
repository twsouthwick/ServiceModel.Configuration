using System.ServiceModel;

namespace ServiceModel.Configuration
{
    public interface IChannelFactoryProvider
    {
        ChannelFactory<T> CreateChannelFactory<T>(string name);
    }
}
