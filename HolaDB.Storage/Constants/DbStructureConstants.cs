using System.Text;

namespace HolaDB.Storage.Constants;

public static class DbStructureConstants
{
    public static readonly byte[] MagicNumber = Encoding.ASCII.GetBytes("HOLADB12");
    public const int PageSize = 8;

    public const string DefaultDbName = "database.hsql";

    public const int DbVersionLength = 16;
    public const int HeaderLength = 40;
}
