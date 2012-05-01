using System;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class Result : IResult
    {
        public Result(string name, string expected, string value, string message, ExceptionType type, ExceptionSeverity severity)
        {
            if(String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (String.IsNullOrEmpty(expected))
                throw new ArgumentNullException("expected");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");

            Name = name;
            Value = value;
            Expected = expected;
            Message = message;

            SetExceptionSeverity(severity);
            SetExceptionType(type);
        }

        #region IResult Members

        public string Name { get; private set; }
        public string Message { get; private set; }
        public string Expected { get; private set; }
        public string Value { get; private set; }
        public ExceptionType Type { get; private set; }
        public ExceptionSeverity Severity { get; private set; }

        public void SetExceptionType(ExceptionType type)
        {
            if(type == ExceptionType.None)
                throw new System.Exception("ExceptionType não pode ser None");

            Type = type;
        }

        public void SetExceptionSeverity(ExceptionSeverity severity)
        {
            if (severity == ExceptionSeverity.None)
                throw new System.Exception("ExceptionSeverity não pode ser None");

            Severity = severity;
        }

        #endregion
    }
}
