using HolaDB.CLI.Models.Options.Version;
using HolaDB.CLI.Services.Interfaces;
using HolaDB.Storage.Service.File;
using Microsoft.Extensions.Options;

namespace HolaDB.CLI.Services;

public class CliClient(IOptions<VersionOptions> VersionOptions, IDatabaseFileManager DatabaseFileManager) : ICliClient
{
    VersionOptions VersionConfiguration = VersionOptions.Value;
    public void Run()
    {
        while (true)
        {
            DatabaseFileManager.ReadOrCreateDatabase(Directory.GetCurrentDirectory(), VersionConfiguration.Db);

            var command = Console.ReadLine();

            // ToDo: implement handling the command

            Console.WriteLine("You called the following command: {0}", command);
        }
    }
}
