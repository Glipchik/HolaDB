using HolaDB.Storage.Models.DbStructure;

namespace HolaDB.Storage.Service.DbStructure;

public interface IHeaderService
{
    void Write(Stream stream, DatabaseHeader databaseHeader);

    DatabaseHeader Read(Stream stream);
}
