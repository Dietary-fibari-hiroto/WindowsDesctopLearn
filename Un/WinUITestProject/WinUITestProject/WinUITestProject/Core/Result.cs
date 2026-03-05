namespace WinUITestProject.Core;

public class Result { 
    public bool IsSuccess { get; }
    public string? Error { get; }
    private Result(bool success, string? error) {
        IsSuccess = success;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Fail(string error) => new(false, error);
}

public class Result<T> {
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool success, T? value, string? error) {
        IsSuccess = success;
        Value = value;
        Error = error;
    }
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Fail(string error) => new(false, default, error);

}