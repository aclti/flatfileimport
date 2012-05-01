using System;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class ValidateLinePositional : IValidate
    {
        private IBlueprintLine _blueprintLine;
        private string _rawDataLine;

        public ValidateLinePositional(string rawLine, IBlueprintLine blueprintLine)
        {
            if (String.IsNullOrEmpty(rawLine))
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
                if (!_blueprintLine.Regex.IsMatch(_rawDataLine))
                {
                    Result = new Result(_blueprintLine.Name, 
                                        _blueprintLine.Regex.ToString(), 
                                        _rawDataLine, 
                                        "A linha não casa com o padrão definido na Regex da Blueprint.", 
                                        ExceptionType.Error, 
                                        ExceptionSeverity.Fatal);

                    return false;
                }

                var sum = _blueprintLine.BlueprintFields.Sum(b => b.Size);

                if(_rawDataLine.Length != sum)
                {
                    Result = new Result(_blueprintLine.Name, 
                                        sum.ToString(""), 
                                        _rawDataLine.Length.ToString(""), 
                                        "A soma do tamanho do campos, definidos na Blueprint, é diferente do tamanho da linha importada.", 
                                        ExceptionType.Error, 
                                        ExceptionSeverity.Fatal);

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
