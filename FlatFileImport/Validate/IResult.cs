using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public interface IResult
    {
        string Name { get; }
        string Message { get; }
        string Expected { get; }
        string Value { get; }
        ExceptionType Type { get; }
        ExceptionSeverity Severity { get; }

        void SetExceptionType(ExceptionType type);
        void SetExceptionSeverity(ExceptionSeverity severity);
    }
}
