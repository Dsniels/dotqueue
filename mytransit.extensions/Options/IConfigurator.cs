using System;

namespace mytransit.extensions.Options;

public interface IConfigurator
{
    IConfigurator AddConsumer<T>() where T : class;
}
