using System;

namespace FlatFileImport.Exception
{
    public class NotSupportedDataTypeException : System.Exception, IParserException
    {
        public NotSupportedDataTypeException (string msg) : base(msg)
        {
            
        }

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
