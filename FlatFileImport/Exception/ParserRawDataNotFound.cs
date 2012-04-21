namespace FlatFileImport.Exception
{
    public class ParserRawDataNotFound: System.Exception, IImporterException
    {
        public ExceptionType Type
        {
            get { return ExceptionType.Error; }
        }

        public ExceptionSeverity Severity
        {
            get { return ExceptionSeverity.Fatal; }
        }
    }
}
