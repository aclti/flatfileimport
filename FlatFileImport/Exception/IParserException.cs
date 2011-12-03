namespace FlatFileImport.Exception
{
    public interface IParserException
    {
        ExceptionType Type { get; }
        ExceptionSeverity Severity { get; }
    }
}
