using HolaDB.Core.Enums;

namespace HolaDB.Core.Models.Lexer;

public sealed record Token(TokenType Type, string Value, int Position, int Length);
