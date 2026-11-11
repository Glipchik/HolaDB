namespace HolaDB.Storage.Models.DbStructure;

/// <summary>
/// Represents some of the data from the database file header.
/// Header structure is:
/// - Magic Number (unique for HolaDB): 8 bytes
/// - Version of HolaDB: 16 bytes
/// - Database ID: 16 bytes
/// - Creation Timestamp: 8 bytes
/// - Page Size: 4 bytes
/// - Reserved: 12 bytes
/// </summary>
public sealed record DatabaseHeader(Guid DatabaseId, long CreatedAt, string DbVersion);