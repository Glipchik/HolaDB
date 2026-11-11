using Microsoft.Extensions.Options;

namespace HolaDB.CLI.Models.Options.Version;

public class VersionOptionsValidator : IValidateOptions<VersionOptions>
{
    public ValidateOptionsResult Validate(string? name, VersionOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(options.Db))
            errors.Add("DB version is not configured");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
