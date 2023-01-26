namespace Gateway.Host.IoC;

using Autofac;
using Gateway.Core;
using Gateway.Core.Abstractions;
using Gateway.Core.Mutators;
using System.Collections.Generic;
using Gateway.Core.Data;
using System;

public class Core : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RequestHandler>()
               .AsSelf();

        builder.RegisterType<ServiceHandler>()
               .AsSelf();

        builder.RegisterType<ServiceValidator>()
               .As<IServiceValidator>()
               .SingleInstance();

        builder.RegisterType<UrlMutator>().As<IHttpRequestMutator>();
        builder.RegisterType<JwtMutator>().As<IHttpRequestMutator>();
        builder.RegisterType<HeaderMutator>().As<IHttpRequestMutator>();

        builder.Register(c =>
        {
            var dict = new Dictionary<Policy, Type>();
            dict.Add(Policy.AddHeader, typeof(AddHeaderPolicy));
            dict.Add(Policy.JwtValidation, typeof(JwtValidationPolicy));

            return dict;
        })
        .AsSelf()
        .SingleInstance();
    }
}