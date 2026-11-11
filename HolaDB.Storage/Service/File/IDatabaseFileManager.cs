namespace HolaDB.Storage.Service.File;

public interface IDatabaseFileManager
{
    void CreateDatabase(string path, string version);

    void ReadOrCreateDatabase(string path, string version);
}
