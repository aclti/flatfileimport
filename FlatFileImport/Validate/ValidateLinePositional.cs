using System;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Exception;
using FlatFileImport.Input;

namespace FlatFileImport.Validate
{
    public class ValidateLinePositional : IValidate
    {
        private IBlueprintLine _blueprintLine;
        private IRawLineAndFields _rawDataLine;

        public ValidateLinePositional(IRawLineAndFields rawLine, IBlueprintLine blueprintLine)
        {
            if (rawLine == null)
                throw new ArgumentNullException("rawLine");

            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _blueprintLine = blueprintLine;
            _rawDataLine = rawLine;
        }

        #region IValidate Members

        public bool IsValid
        {
            get
            {
                if (!_blueprintLine.Regex.IsMatch(_rawDataLine.Value))
                {
                    Result = new Result("A linha não casa com o padrão definido na Regex da Blueprint.", ExceptionType.Error, ExceptionSeverity.Fatal)
                    {
                        LineName = _blueprintLine.Name,
                        LineNumber = _rawDataLine.Number,
                        Value = _rawDataLine.Value,
                        Expected = _blueprintLine.Regex.ToString(),
                    };

                    return false;
                }

                var sum = _blueprintLine.BlueprintFields.Sum(b => b.Size);

                if(_rawDataLine.Value.Length != sum)
                {
                    Result = new Result("A soma do tamanho do campos, definidos na Blueprint, é diferente do tamanho da linha importada.", ExceptionType.Error, ExceptionSeverity.Fatal)
                    {
                        LineName = _blueprintLine.Name,
                        LineNumber = _rawDataLine.Number,
                        Value = _rawDataLine.Value.Length.ToString(""),
                        Expected = sum.ToString(""),
                    };

                    return false;
                }

                return true;
            }
        }

        public IResult Result { get; private set; }

        #endregion

        //private readonly string[] _rawDataCollection;
        //private readonly IBlueprintLine _blueprintLine;

        //public ValidateLine(string[] rawDataCollection, IBlueprintLine blueprintLine)
        //{
        //    if (rawDataCollection == null)
        //        throw new ArgumentNullException("rawDataCollection");

        //    if (blueprintLine == null)
        //        throw new ArgumentNullException("blueprintLine");

        //    _blueprintLine = blueprintLine;
        //    _rawDataCollection = rawDataCollection;
        //}

        //#region IValidate Members

        //public IResult ValidateResult
        //{
        //    get { return Valid(); }
        //}

        //public bool IsValid()
        //{
        //    return Valid().Success;
        //}

        //public IResult Valid()
        //{
        //    var result = new ValidResult { Success = true };

        //    if (_blueprintLine.BlueprintFields.Count != _rawDataCollection.Length)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendLine();
        //        sb.AppendFormat("Quantidade de campos definidos na blueprint: {0}", _blueprintLine.BlueprintFields.Count);
        //        sb.AppendLine();
        //        sb.AppendFormat("Quantidade de campos no registro: {0}", _rawDataCollection.Length);
        //        sb.AppendLine();
        //        sb.AppendFormat("Linha do registro: {0}", _rawDataCollection);
        //        sb.AppendLine();
        //        sb.AppendFormat("[NOME: {0}][VALOR: {1}]", _blueprintLine.Name, String.Join("|", _rawDataCollection));
        //        sb.AppendLine();

        //        if (_blueprintLine.BlueprintFields.Count > _rawDataCollection.Length)
        //        {
        //            result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false};
        //        }
        //        else if (_blueprintLine.BlueprintFields.Count < _rawDataCollection.Length)
        //        {
        //            if (/*_blueprintLine.Mandatory*/true)
        //                result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error, Success = false };
        //            else
        //                result = new ValidResult { Message = sb.ToString(), Severity = ExceptionSeverity.Critical, Type = ExceptionType.Warnning, Success = false };
        //        }

        //    }

        //    return result;
        //}

        //#endregion
    }
}
