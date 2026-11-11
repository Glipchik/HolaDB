namespace HolaDB.CLI.Models.Options.Version;

public sealed record VersionOptions
{
    public const string SectionName = "Version";

    public string Db { get; set; } = string.Empty;
}
