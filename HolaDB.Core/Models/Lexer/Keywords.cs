using HolaDB.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HolaDB.Core.Models.Lexer;

public static class Keywords
{
    public static readonly Dictionary<string, TokenType> KeywordsList =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["SELECT"] = TokenType.Select,
            ["FROM"] = TokenType.From,
            ["WHERE"] = TokenType.Where,
            ["AND"] = TokenType.And,
            ["OR"] = TokenType.Or,
            ["NOT"] = TokenType.Not,
            ["NULL"] = TokenType.NullLiteral,
            ["INT"] = TokenType.Int,
            ["VARCHAR"] = TokenType.Varchar
        };
}
