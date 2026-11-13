namespace HolaDB.Core.Enums;

/// <summary>
/// Represents all HolaSQL Token types.
/// It is suggested to NOT rely on the order of types in the enum as it will probably change.
/// </summary>
public enum TokenType
{
    // Special
    None = 0,
    EndOfFile,

    // Identifiers
    Identifier,

    // Keywords
    Select,
    From,
    Where,

    // Logical operators
    And,
    Or,
    Not,

    // Comparison operators
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,

    // Literals
    IntegerLiteral,
    StringLiteral,
    NullLiteral,

    // Data types
    Int,
    Varchar,

    // Symbols
    Asterics,
    OpenParenthesis,
    CloseParenthesis,
    Comma,
    Semicolon
}
