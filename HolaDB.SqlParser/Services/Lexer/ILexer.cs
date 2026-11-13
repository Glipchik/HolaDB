using HolaDB.Core.Models.Lexer;
using HolaDB.Core.Models.Result;

namespace HolaDB.SqlParser.Services.Lexer;

public interface ILexer
{
    IReadOnlyList<ValueResult<Token>> GetTokenSequence(string command);
}
