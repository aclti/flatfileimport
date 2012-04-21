using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class ValidResult
    {
        public string Message { set; get; }
        public ExceptionType Type { set; get; }
        public ExceptionSeverity Severity { set; get; }
        public bool Success { set; get; }
    }
}
