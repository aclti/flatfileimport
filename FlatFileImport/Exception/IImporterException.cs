namespace FlatFileImport.Exception
{
    public interface IImporterException
    {
        ExceptionType Type { get; }
        ExceptionSeverity Severity { get; }
    }
}
