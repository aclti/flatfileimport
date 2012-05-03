using System;
using System.Text;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class Result : IResult
    {
        private StringBuilder _message;

        public Result(string description, ExceptionType type, ExceptionSeverity severity)
        {
            if(String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");


            Description = description;
            SetExceptionSeverity(severity);
            SetExceptionType(type);
        }

        public Result(int lineNumber, string description, ExceptionType type, ExceptionSeverity severity) : this(description, type, severity)
        {
            LineNumber = lineNumber;
            SetExceptionSeverity(severity);
            SetExceptionType(type);
        }

        #region IResult Members

        public string LineName { get; set; }
        public string FieldName { get; set; }
        public string Description { get; set; }
        public string Expected { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }
        public ExceptionType Type { get; private set; }
        public ExceptionSeverity Severity { get; private set; }

        public string FullMessage
        {
            get
            {
                _message = new StringBuilder();

                if (LineNumber > 0)
                    _message.AppendFormat("** [ Linha n°: {0} ] ", LineNumber);

                _message.AppendFormat("- {0} | ", Description);

                if (!String.IsNullOrEmpty(LineName))
                    _message.AppendFormat("Registro: {0} | ", LineName);

                if (!String.IsNullOrEmpty(FieldName))
                    _message.AppendFormat("Campo: {0} | ", FieldName);

                if (!String.IsNullOrEmpty(Expected))
                    _message.AppendFormat("Valor Esperado: {0} | ", Expected);

                if (!String.IsNullOrEmpty(Value))
                    _message.AppendFormat("Valor Recebido: {0} | ", Value);

                _message.AppendFormat("Tipo da Exceção: {0} | ", Type);
                _message.AppendFormat("Gravidade do Erro: {0} | ", Severity);
                _message.AppendLine();

                _message.Remove(_message.Length - 4, 3);
                return _message.ToString();
            }
        }

        public string Message
        {
            get
            {
                _message = new StringBuilder();

                if (LineNumber > 0)
                    _message.AppendFormat("** [ Linha n°: {0} ] ", LineNumber);

                _message.AppendFormat("- {0} | ", Description);

                if(!String.IsNullOrEmpty(LineName))
                    _message.AppendFormat("Registro: {0} | ", LineName);

                if (!String.IsNullOrEmpty(FieldName))
                    _message.AppendFormat("Campo: {0} | ", FieldName);

                if (!String.IsNullOrEmpty(Expected))
                    _message.AppendFormat("Valor Esperado: {0} | ", Expected);

                if (!String.IsNullOrEmpty(Value))
                    _message.AppendFormat("Valor Recebido: {0} | ", Value);

               _message.AppendLine();

                _message.Remove(_message.Length - 4, 3);
                return _message.ToString();
            }
        }

        public string ShortMessage
        {
            get
            {
                _message = new StringBuilder();

                if (LineNumber > 0)
                    _message.AppendFormat("** [ Linha n°: {0} ] ", LineNumber);

                _message.AppendFormat("- {0} | ", Description);

                if (!String.IsNullOrEmpty(Value))
                    _message.AppendFormat("Valor Recebido: {0} | ", Value);

                _message.AppendLine();

                _message.Remove(_message.Length - 4, 3);
                return _message.ToString();
            }
        } 


        public void SetExceptionType(ExceptionType type)
        {
            if (type == ExceptionType.None)
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