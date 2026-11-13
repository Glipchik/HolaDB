using HolaDB.Core.Enums;

namespace HolaDB.Core.Models.Result;

public record ValueResult<T> : Result
{
    public T? Value { get; set; }

    protected ValueResult(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static ValueResult<T> Success(T value) => new(true, value, null);
    public static new ValueResult<T> Failure(Error error) => new(false, default, error ?? throw new ArgumentNullException(nameof(error)));
}

public record Result
{
    public bool IsSuccess { get; set; }
    public Error? Error { get; set; }

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    public static implicit operator Result(Error error) => Failure(error);
}

public sealed record Error(ErrorType ErrorType, string Message);

public static class Errors
{
    public static Error NotSupportedTokenType { get; } = new(ErrorType.NotSupportedTokenType, "Not supported token type in the sequence");
    public static Error EmptyCommand { get; } = new(ErrorType.EmptyCommand, "The HSQL command is empty");
}