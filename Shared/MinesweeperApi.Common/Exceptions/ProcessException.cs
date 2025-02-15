using Microsoft.AspNetCore.Identity;
namespace MinesweeperApi.Common.Exceptions;

public class ProcessException : Exception
{
    public string Code { get; }

    public string Name { get; }


    public ProcessException()
    {
    }

    public ProcessException(string message) : base(message)
    {
    }

    public ProcessException(Exception inner) : base(inner.Message, inner)
    {
    }

    public ProcessException(string message, Exception inner) : base(message, inner)
    {
    }

    public ProcessException(string code, string message) : base(message)
    {
        Code = code;
    }

    public ProcessException(string code, string message, Exception inner) : base(message, inner)
    {
        Code = code;
    }

    public static void ThrowIf(Func<bool> predicate, string message)
    {
        if (predicate.Invoke())
            throw new ProcessException(message);
    }

    public static void ThrowIfNotSuccessful(IdentityResult result)
    {
        throw new NotImplementedException();
    }
}
