using System;

namespace FlatFileImport.Exception
{
    public class WrongTypeFileException : System.Exception, IParserException
    {
        public WrongTypeFileException(string message) :base (message)
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
