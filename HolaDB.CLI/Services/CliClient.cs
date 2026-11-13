using HolaDB.CLI.Models.Options.Version;
using HolaDB.CLI.Services.Interfaces;
using HolaDB.SqlParser.Services.Lexer;
using HolaDB.Storage.Service.File;
using Microsoft.Extensions.Options;
using System.Text;

namespace HolaDB.CLI.Services;

public class CliClient(IOptions<VersionOptions> VersionOptions, IDatabaseFileManager DatabaseFileManager, ILexer Lexer) : ICliClient
{
    VersionOptions VersionConfiguration = VersionOptions.Value;
    public void Run()
    {
        while (true)
        {
            DatabaseFileManager.ReadOrCreateDatabase(Directory.GetCurrentDirectory(), VersionConfiguration.Db);

            var command = Console.ReadLine();

            if (command is null)
            {
                Console.WriteLine("The command is empty");
                continue;
            }

            // ToDo: implement handling the command

            var tokens = Lexer.GetTokenSequence(command);
            StringBuilder result = new("Lexed tokens:\n");

            foreach (var token in tokens)
            {
                result.Append($"IsSuccess: {token.IsSuccess}, Value or Error: ${(token.IsSuccess ? token.Value!.Value : token.Error!.Message)}\n");
            }

            Console.WriteLine("You called the following command: {0}", command);
            Console.WriteLine($"The sequence is: {result}");
        }
    }
}
