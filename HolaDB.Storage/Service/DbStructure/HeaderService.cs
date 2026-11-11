using HolaDB.Storage.Constants;
using HolaDB.Storage.Models.DbStructure;
using System.Text;

namespace HolaDB.Storage.Service.DbStructure;

internal class HeaderService() : IHeaderService
{
    public DatabaseHeader Read(Stream stream)
    {
        using var reader = new BinaryReader(
            stream,
            Encoding.UTF8,
            leaveOpen: true);

        var magicNumber = reader.ReadBytes(8);

        if(!magicNumber.SequenceEqual(DbStructureConstants.MagicNumber))
        {
            throw new InvalidDataException(
                "Invalid database file: incorrect Magic Number.");
        }

        var dbVersionBytes = reader.ReadBytes(16);
        var dbVersion = Encoding.ASCII.GetString(dbVersionBytes);

        if (dbVersionBytes.Length != 16 || dbVersion is null)
        {
            throw new InvalidDataException(
                "Invalid database file: incomplete DB Version.");
        }

        var databaseIdBytes = reader.ReadBytes(16);

        if (databaseIdBytes.Length != 16)
        {
            throw new InvalidDataException(
                "Invalid database file: incomplete Database ID.");
        }

        var databaseId = new Guid(databaseIdBytes);

        var createdAt = reader.ReadInt64();

        var pageSize = reader.ReadInt32();

        if (pageSize != 8)
        {
            throw new InvalidDataException(
                $"Unsupported page size: {pageSize}.");
        }
        var reserved = reader.ReadBytes(12);

        if (reserved.Length != 12)
        {
            throw new InvalidDataException(
                "Invalid database file: incomplete header.");
        }

        return new DatabaseHeader(
            DatabaseId: databaseId,
            CreatedAt: createdAt,
            DbVersion: dbVersion);
    }

    public void Write(Stream stream, DatabaseHeader databaseHeader)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

        writer.Write(DbStructureConstants.MagicNumber);

        var dbVersion = new byte[DbStructureConstants.DbVersionLength];
        Encoding.ASCII.GetBytes(databaseHeader.DbVersion, dbVersion);
        writer.Write(dbVersion);

        writer.Write(databaseHeader.DatabaseId.ToByteArray());
        writer.Write(databaseHeader.CreatedAt);
        writer.Write(DbStructureConstants.PageSize);
        writer.Write(new byte[12]);

        writer.Flush();
    }
}
