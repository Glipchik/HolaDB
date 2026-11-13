using HolaDB.Core.Enums;
using HolaDB.Core.Models.Lexer;
using HolaDB.Core.Models.Result;
using System.Text;

namespace HolaDB.SqlParser.Services.Lexer;

public sealed class Lexer : ILexer
{
    public IReadOnlyList<ValueResult<Token>> GetTokenSequence(string command)
    {
        var index = 0;
        var tokens = new List<ValueResult<Token>>();

        if (string.IsNullOrEmpty(command))
        {
            return [ValueResult<Token>.Failure(Errors.EmptyCommand)];
        }

        while (command.Length > index) 
        {
            SkipWhitespaces(command, ref index);

            var token = ReadNextToken(command, index);
            tokens.Add(token);

            if (!token.IsSuccess)
                return tokens;

            index += token.Value!.Length;
        }

        Token eofToken = new(TokenType.EndOfFile, string.Empty, index, 0);
        tokens.Add(ValueResult<Token>.Success(eofToken));

        return tokens;
    }

    private static void SkipWhitespaces(string sequence, ref int currentIndex)
    {
        while (currentIndex < sequence.Length && char.IsWhiteSpace(sequence[currentIndex]))
            currentIndex++;
    }

    private static ValueResult<Token> ReadNextToken(string sequence, int currentIndex)
    {
        if (currentIndex == sequence.Length)
            return ValueResult<Token>.Success(new(TokenType.EndOfFile, "\0", currentIndex, 1));

        var nextElement = sequence[currentIndex];

        if (char.IsLetter(nextElement) || nextElement == '_')
            return ReadIdentifierOrKeyword(sequence, currentIndex);

        if (char.IsDigit(nextElement))
            return ReadNumber(sequence, currentIndex);

        return nextElement switch
        {
            '\'' => ReadString(sequence, currentIndex),
            '=' => ReadEqual(sequence, currentIndex),
            '!' or '>' or '<' => ReadComparisonOperator(sequence, currentIndex),
            '*' => ReadAsterics(currentIndex),
            '(' => ReadOpenParenthesis(currentIndex),
            ')' => ReadClosedParenthesis(currentIndex),
            _ => ValueResult<Token>.Failure(Errors.NotSupportedTokenType)
        };
    }

    private static ValueResult<Token> ReadIdentifierOrKeyword(string sequence, int index)
    {
        var currentIndex = index;

        while (currentIndex < sequence.Length &&
           (char.IsLetterOrDigit(sequence[currentIndex]) ||
            sequence[currentIndex] == '_'))
        {
            currentIndex++;
        }

        var lexeme = sequence[index..currentIndex];

        var tokenType = Keywords.KeywordsList.TryGetValue(lexeme, out var keywordType)
            ? keywordType
            : TokenType.Identifier;

        return ValueResult<Token>.Success(
            new Token(
                tokenType,
                lexeme,
                index,
                currentIndex - index));
    }

    private static ValueResult<Token> ReadString(string sequence, int index)
    {
        var currentIndex = index + 1;
        var escapeElementIndexes = new List<int>();

        while (currentIndex < sequence.Length)
        {
            if (sequence[currentIndex] == '\'')
            {
                if (currentIndex + 1 < sequence.Length &&
                    sequence[currentIndex + 1] == '\'')
                {
                    escapeElementIndexes.Add(currentIndex);
                    currentIndex += 2;
                    continue;
                }

                currentIndex++;
                break;
            }

            currentIndex++;
        }

        if (currentIndex == sequence.Length &&
            sequence[currentIndex - 1] != '\'')
        {
            return ValueResult<Token>.Failure(Errors.NotSupportedTokenType);
        }

        var builder = new StringBuilder();

        for (var i = index + 1; i < currentIndex - 1; i++)
        {
            if (escapeElementIndexes.Contains(i))
            {
                builder.Append('\'');
                i++;
                continue;
            }

            builder.Append(sequence[i]);
        }

        return ValueResult<Token>.Success(
            new Token(
                TokenType.StringLiteral,
                builder.ToString(),
                index,
                currentIndex - index));
    }

    private static ValueResult<Token> ReadNumber(string sequence, int index)
    {
        var currentIndex = index;
        bool hasNext;
        char nextElement;

        do
        {
            hasNext = HasNext(currentIndex, sequence);
            nextElement = Peek(sequence, currentIndex, hasNext);
            if (!hasNext && nextElement == '.')
                return ValueResult<Token>.Failure(Errors.NotSupportedTokenType);

            currentIndex++;
        }
        while (hasNext && (char.IsDigit(nextElement) || nextElement == '.'));

        return ValueResult<Token>.Success(new(TokenType.IntegerLiteral, sequence[index..currentIndex], index, currentIndex - index));
    }

    private static ValueResult<Token> ReadComparisonOperator(string sequence, int currentIndex)
    {
        var current = sequence[currentIndex];

        var hasNext = HasNext(currentIndex, sequence);
        var next = Peek(sequence, currentIndex + 1, hasNext);

        return current switch
        {
            '>' when next == '='
                => ValueResult<Token>.Success(new(TokenType.GreaterThanOrEqual, ">=", currentIndex, 2)),
            '>'
                => ValueResult<Token>.Success(new(TokenType.GreaterThan, ">", currentIndex, 1)),
            '<' when next == '='
                => ValueResult<Token>.Success(new(TokenType.LessThanOrEqual, "<=", currentIndex, 2)),
            '<'
                => ValueResult<Token>.Success(new(TokenType.LessThan, "<", currentIndex, 1)),
            '!' when next == '='
                => ValueResult<Token>.Success(new(TokenType.NotEqual, "!=", currentIndex, 2)),
            _
                => ValueResult<Token>.Failure(Errors.NotSupportedTokenType)
        };
    }

    private static ValueResult<Token> ReadClosedParenthesis(int currentIndex)
        => ValueResult<Token>.Success(new(TokenType.CloseParenthesis, ")", currentIndex, 1));

    private static ValueResult<Token> ReadOpenParenthesis(int currentIndex)
        => ValueResult<Token>.Success(new(TokenType.OpenParenthesis, "(", currentIndex, 1));

    private static ValueResult<Token> ReadAsterics(int currentIndex)
        => ValueResult<Token>.Success(new(TokenType.Asterics, "*", currentIndex, 1));

    private static ValueResult<Token> ReadEqual(string sequence, int currentIndex)
        => ValueResult<Token>.Success(new(TokenType.Equal, "=", currentIndex, 1));

    private static bool HasNext(int currentIndex, string sequence)
        => currentIndex + 1 < sequence.Length;

    private static char Peek(string sequence, int index, bool hasNext)
        => hasNext ? sequence[index] : '\0';
}
