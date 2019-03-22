using System;

namespace ServiceModel.Configuration
{
    public interface ITypeMapper
    {
        Type GetType(string name);
    }
}
