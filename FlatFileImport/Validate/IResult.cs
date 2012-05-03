using System.Text;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public interface IResult
    {
        string LineName { set; get; }
        string FieldName { set; get; }
        string Description { set; get; }
        string Expected { set; get; }
        string Value { set; get; }
        int LineNumber { set; get; }
        ExceptionType Type { get; }
        ExceptionSeverity Severity { get; }

        string FullMessage { get; }
        string ShortMessage { get; }
        string Message { get; }

        void SetExceptionType(ExceptionType type);
        void SetExceptionSeverity(ExceptionSeverity severity);
    }
}
