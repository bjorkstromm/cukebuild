using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CUKE.Tool.Commands.Setting;
using Gherkin;
using Gherkin.Ast;
using Spectre.Console.Cli;

namespace CUKE.Tool.Commands
{
    public class BuildCommand : AsyncCommand<BuildSettings>
    {
        private ILogger Logger { get; }

        public override async Task<int> ExecuteAsync(CommandContext context, BuildSettings settings)
        {
            Parser parser = new();
            
            Logger.LogInformation("Compiling source {SourceFile}...", settings.SourceFile);

            var doc = parser.Parse(settings.SourceFile);

            if (string.IsNullOrWhiteSpace(doc?.Feature?.Name))
            {
                Logger.LogError("Failed to parse source");

                return 500;
            }

            using (Logger.BeginScope(doc.Feature.Name))
            {
                Logger.LogInformation("Executing build...");

                foreach (Scenario scenario in doc.Feature.Children.OfType<Scenario>())
                {
                    using (Logger.BeginScope(scenario.Name))
                    {
                        foreach (var scenarioStep in scenario.Steps)
                        {
                            Logger.LogInformation("{Keyword}: {Argument}", scenarioStep.Keyword, scenarioStep.Text);
                        }
                    }
                }
            }

            return await Task.FromResult(0);
        }

        public BuildCommand(ILogger<BuildCommand> logger)
        {
            Logger = logger;
        }
    }
}