using HolaDB.CLI.Models.Options.Version;
using HolaDB.CLI.Services;
using HolaDB.CLI.Services.Interfaces;
using HolaDB.SqlParser.DI;
using HolaDB.Storage.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();

services.Configure<VersionOptions>(
    configuration.GetSection(VersionOptions.SectionName));
services.AddSingleton<IValidateOptions<VersionOptions>, VersionOptionsValidator>();

services.AddStorageDependencies();
services.AddParserDependencies();
services.AddSingleton<ICliClient, CliClient>();


var provider = services.BuildServiceProvider();

try
{
    var client = provider.GetRequiredService<ICliClient>();
    client.Run();
}
catch (OptionsValidationException ex)
{
    Console.WriteLine("Error appeared because of invalid settings:");

    foreach (var failure in ex.Failures)
    {
        Console.WriteLine($"- {failure}");
    }

    return;
}
catch (Exception ex)
{
    Console.WriteLine("There appeared an exception: {0}", ex.Message);
}