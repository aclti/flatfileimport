using System;
using FlatFileImport.Core;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class ValidateLineSeparatedCharacter : IValidate
    {
        private IBlueprintLine _blueprintLine;
        private string _rawDataLine;

        public ValidateLineSeparatedCharacter(string rawLine, IBlueprintLine blueprintLine)
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

                var rawColl = _rawDataLine.Split(_blueprintLine.Blueprint.BluePrintCharSepartor);

                if (rawColl.Length != _blueprintLine.BlueprintFields.Count)
                {
                    Result = new Result(_blueprintLine.Name, 
                                        _blueprintLine.BlueprintFields.Count.ToString(""), 
                                        rawColl.Length.ToString(""), 
                                        "A quantidade de campos definido na Blueprint não é mesma dos campos da linha importada.", 
                                        ExceptionType.Error, 
                                        ExceptionSeverity.Fatal);

                    return false;
                }

                return true;
            }
        }

        public IResult Result { get; private set; }

        #endregion
    }
}
