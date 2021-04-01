using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CUKE.Tool.Commands.Setting
{
    public class BuildSettings : CommandSettings
    {
        [CommandArgument(0, "<sourcefile>")]
        [Description("Source file path")]
        public string SourceFile { get; set; } = null!;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(SourceFile) || !System.IO.File.Exists(SourceFile))
            {
                return ValidationResult.Error($"Specified path doesn't exists: {SourceFile}");
            }

            return base.Validate();
        }
    }
}