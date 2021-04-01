using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CUKE.Tool.Commands;
using Spectre.Console.Cli;
using Spectre.Cli.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection()
    .AddLogging(configure =>
            configure
                .AddSimpleConsole(opts => {
                    opts.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                    opts.SingleLine = true;
                    opts.IncludeScopes = false;
                })
    );

using var registrar = new DependencyInjectionRegistrar(serviceCollection);
var app = new CommandApp(registrar);

app.Configure(
    config =>
    {
        config.ValidateExamples();

        config.AddCommand<BuildCommand>("build")
                .WithDescription("Example console command.")
                .WithExample(new[] { "build" });
    });

return await app.RunAsync(args);