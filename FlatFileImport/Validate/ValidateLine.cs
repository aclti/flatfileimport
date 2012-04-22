using System;
using FlatFileImport.Core;
using FlatFileImport.Exception;
using System.Text;

namespace FlatFileImport.Validate
{
    public class ValidateLine : IValidate
    {
        private readonly string[] _rawDataCollection;
        private readonly IBlueprintLine _blueprintLine;

        public ValidateLine(string[] rawDataCollection, IBlueprintLine blueprintLine)
        {
            if (rawDataCollection == null)
                throw new ArgumentNullException("rawDataCollection");

            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _blueprintLine = blueprintLine;
            _rawDataCollection = rawDataCollection;
        }

        #region IValidate Members

        public ValidResult ValidateResult
        {
            get { return Valid(); }
        }

        public bool IsValid()
        {
            return Valid().Success;
        }

        public ValidResult Valid()
        {
            var result = new ValidResult { Success = true };

            if (_blueprintLine.BlueprintFields.Count != _rawDataCollection.Length)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendFormat("Quantidade de campos definidos na blueprint: {0}", _blueprintLine.BlueprintFields.Count);
                sb.AppendLine();
                sb.AppendFormat("Quantidade de campos no registro: {0}", _rawDataCollection.Length);
                sb.AppendLine();
                sb.AppendFormat("Linha do registro: {0}", _rawDataCollection);
                sb.AppendLine();
                sb.AppendFormat("[NOME: {0}][VALOR: {1}]", _blueprintLine.Name, String.Join("|", _rawDataCollection));
                sb.AppendLine();

                if (_blueprintLine.BlueprintFields.Count > _rawDataCollection.Length)
                {
                    result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false};
                }
                else if (_blueprintLine.BlueprintFields.Count < _rawDataCollection.Length)
                {
                    if (/*_blueprintLine.Mandatory*/true)
                        result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false };
                    else
                        result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Critical, Type = ExceptionType.Warnning, Success = false };
                }

            }

            return result;
        }

        #endregion
    }
}
