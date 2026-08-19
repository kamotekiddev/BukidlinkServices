using Microsoft.AspNetCore.Builder;
using Serilog;

namespace BuildingBlocks.Extensions;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceName", context.HostingEnvironment.ApplicationName)
                .WriteTo.Console();

            var seqUrl = context.Configuration["Seq:ServerUrl"];

            if (!string.IsNullOrWhiteSpace(seqUrl)) configuration.WriteTo.Seq(seqUrl);
        });

        return builder;
    }
}