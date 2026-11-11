using HolaDB.Storage.Constants;
using HolaDB.Storage.Models.DbStructure;
using HolaDB.Storage.Service.DbStructure;

namespace HolaDB.Storage.Service.File;

public class DatabaseFileManager(IHeaderService HeaderService) : IDatabaseFileManager
{
    public void CreateDatabase(string path, string version)
    {
        var databaseFilePath = Path.Combine(
            path,
            DbStructureConstants.DefaultDbName);

        if (System.IO.File.Exists(databaseFilePath))
        {
            throw new IOException($"Database already exists: {path}");
        }

        var databaseId = Guid.NewGuid();

        var header = new DatabaseHeader(databaseId, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), version);

        Directory.CreateDirectory(path);

        using var stream = new FileStream(
            databaseFilePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None);

        HeaderService.Write(stream, header);
    }

    public void ReadOrCreateDatabase(string path, string version)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(
                $"Database directory not found: {path}");
        }

        var databaseFilePath = Path.Combine(
            path,
            DbStructureConstants.DefaultDbName);

        if (!System.IO.File.Exists(databaseFilePath))
        {
            CreateDatabase(path, version);
            return;
        }

        using var stream = new FileStream(
            databaseFilePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read);

        if (stream.Length < DbStructureConstants.HeaderLength)
        {
            throw new InvalidDataException(
                "Database file is too small.");
        }

        HeaderService.Read(stream);
    }
}
