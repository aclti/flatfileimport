using System;
using System.Collections.Generic;
using FlatFileImport.Exception;
using FlatFileImport.Process;
using System.Text;

namespace FlatFileImport.Validate
{
    public class ValidateLine : IValidate
    {
        private readonly string[] _rawDataCollection;
        private readonly BlueprintLine _blueprintLine;

        public ValidateLine(string[] rawDataCollection, BlueprintLine blueprintLine)
        {
            if (rawDataCollection == null)
                throw new ArgumentNullException("rawDataCollection");

            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _blueprintLine = blueprintLine;
            _rawDataCollection = rawDataCollection;
        }

        #region IValidate Members

        public ValidateResult ValidateResult
        {
            get { return Valid(); }
        }

        public bool IsValid()
        {
            return Valid().Success;
        }

        public ValidateResult Valid()
        {
            var result = new ValidateResult { Success = true };

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
                sb.AppendFormat("[NOME: {0}][VALOR: {1}]", _blueprintLine.Class, String.Join("|", _rawDataCollection));
                sb.AppendLine();

                if (_blueprintLine.BlueprintFields.Count > _rawDataCollection.Length)
                {
                    result = new ValidateResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false};
                }
                else if (_blueprintLine.BlueprintFields.Count < _rawDataCollection.Length)
                {
                    if (_blueprintLine.Mandatory)
                        result = new ValidateResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false };
                    else
                        result = new ValidateResult { Message = sb.ToString(), Severity = ExceptionSeverity.Critical, Type = ExceptionType.Warnning, Success = false };
                }

            }

            return result;
        }

        #endregion
    }
}
