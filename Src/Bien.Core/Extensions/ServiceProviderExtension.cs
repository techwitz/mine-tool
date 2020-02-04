using System;

namespace System
{
    public static class ServiceProviderExtension
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(TService));
            if (service == null) return default(TService);
            else return (TService)service;
        }
    }
}
