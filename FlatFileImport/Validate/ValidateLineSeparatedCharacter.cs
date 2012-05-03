using System;
using FlatFileImport.Core;
using FlatFileImport.Exception;
using FlatFileImport.Input;

namespace FlatFileImport.Validate
{
    public class ValidateLineSeparatedCharacter : IValidate
    {
        private IBlueprintLine _blueprintLine;
        private IRawLine _rawDataLine;

        public ValidateLineSeparatedCharacter(IRawLine rawLine, IBlueprintLine blueprintLine)
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

                //TODO Analisar o encapsulamento , com um pattern, da codigo que monta a lista de RawFileds
                var rawColl = _rawDataLine.Value.Split(_blueprintLine.Blueprint.BluePrintCharSepartor);

                if (rawColl.Length < _blueprintLine.BlueprintFields.Count)
                {
                    Result = new Result("A quantidade de campos é menor que a quantidade de campos definido na Blueprint.", ExceptionType.Error, ExceptionSeverity.Fatal)
                                 {
                                    LineName = _blueprintLine.Name,
                                    LineNumber = _rawDataLine.Number,
                                     Value = rawColl.Length.ToString(""),
                                     Expected = _blueprintLine.BlueprintFields.Count.ToString(""),
                                 };

                    return false;
                }

                if (rawColl.Length > _blueprintLine.BlueprintFields.Count)
                {
                    Result = new Result("A quantidade de campos é maior que a quantidade de campos definido na Blueprint. Dados não serão importados.", ExceptionType.Error, ExceptionSeverity.Fatal)
                    {
                        LineName = _blueprintLine.Name,
                        LineNumber = _rawDataLine.Number,
                        Value = rawColl.Length.ToString(""),
                        Expected = _blueprintLine.BlueprintFields.Count.ToString(""),
                    };

                    return false;
                }

                return true;
            }
        }

        public IResult Result { get; private set; }

        #endregion
    }
}
