namespace FlatFileImport.Exception
{
    public class ParserRawDataNotFound: System.Exception, IParserException
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
