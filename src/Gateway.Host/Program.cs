using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Gateway.Host.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Host
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>((_, b) =>
       {
           b.RegisterModule<Core>();
           b.RegisterModule<IO>();
       });

builder.Services.AddControllers();
builder.Services.AddLogging()
                .AddHttpClient()
                .AddMemoryCache();

var app = builder.Build();

app.MapControllers();
app.Run();
