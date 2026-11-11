using HolaDB.Storage.Service.DbStructure;
using HolaDB.Storage.Service.File;
using Microsoft.Extensions.DependencyInjection;

namespace HolaDB.Storage.DI;

public static class StorageDependencies
{
    public static void AddStorageDependencies(this IServiceCollection services)
    {
        services.AddScoped<IHeaderService, HeaderService>();


        services.AddScoped<IDatabaseFileManager, DatabaseFileManager>();
    }
}
