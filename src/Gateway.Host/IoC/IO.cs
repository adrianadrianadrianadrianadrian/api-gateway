namespace Gateway.Host.IoC;

using Autofac;
using Gateway.IO;
using Gateway.Host;
using Microsoft.Extensions.Logging;
using Gateway.Core.Data;
using Gateway.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Gateway.Contracts;
using System.Collections.Generic;
using System;

public class IO : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
        builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));

        builder.RegisterAdapter<IConfiguration, Settings>(c =>
        {
            var settings = new Settings();
            c.Bind(settings);
            return settings;
        })
        .As<Settings>()
        .SingleInstance();

        builder.Register(c =>
        {
            var settings = c.Resolve<Settings>();
            var loggerImpl = c.Resolve<ILogger<ServiceReader>>();
            var cacheImpl = new MemoryCache<Service>(c.Resolve<IMemoryCache>(), settings.ServiceCacheExpiryInSeconds);
            var policies = c.Resolve<Dictionary<Policy, Type>>();

            var reader = new ServiceReader(
                settings.BlobConnectionString,
                settings.ServiceBlobContainerName,
                loggerImpl,
                policies
            );

            return new CachedServiceReader(cacheImpl, reader);
        })
        .As<IServiceReader>();

        builder.Register(c =>
        {
            var settings = c.Resolve<Settings>();
            var loggerImpl = c.Resolve<ILogger<ServiceWriter>>();

            return new ServiceWriter(
                settings.BlobConnectionString,
                settings.ServiceBlobContainerName,
                loggerImpl
            );
        })
        .As<IServiceWriter>();

        builder.Register(c =>
        {
            var settings = c.Resolve<Settings>();
            return new JwtValidator(settings.JwtSecret);
        })
        .As<IJwtValidator<AuthenticatedUser>>();
    }
}