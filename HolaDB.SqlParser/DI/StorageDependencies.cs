using HolaDB.SqlParser.Services.Lexer;
using Microsoft.Extensions.DependencyInjection;

namespace HolaDB.SqlParser.DI;

public static class StorageDependencies
{
    public static void AddParserDependencies(this IServiceCollection services)
    {
        services.AddScoped<ILexer, Lexer>();
    }
}
