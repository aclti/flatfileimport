using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public class ParserSeparatedCharacter : IParser
    {
        private string _rawDataLine;
        private string[] _rawDataColl;
        private IBlueprintLine _blueprintLine;
        private IParsedObjetct _data;
        private Converter _converter;
        private IValidate _validate;
        private List<IResult> _results;

        public ParserSeparatedCharacter()
        {
            _results = new List<IResult>();
        }

        #region IParser Members

        public void SetDataToParse(string rawLine)
        {
            if (String.IsNullOrEmpty(rawLine))
                throw new ArgumentNullException("rawLine");

            _rawDataLine = rawLine;
            _rawDataLine = NormalizeRawData();
            _rawDataColl = _rawDataLine.Split(_blueprintLine.Blueprint.BluePrintCharSepartor);
        }

        public void SetBlueprintLine(IBlueprintLine blueprintLine)
        {
            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _blueprintLine = blueprintLine;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedData(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            _data = new ParsedData(_blueprintLine.Name, parent);

            foreach (var fields in _blueprintLine.BlueprintFields)
            {
                var data = _rawDataColl[fields.Position];

                _converter = Converter.GetConvert(fields.Type);
                _converter.Init(fields, data);

                _data.AddField(fields.Name, _converter.Data, fields.Type);
            }

            return _data;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedLine(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            _data = new ParsedLine(_blueprintLine.Name, parent);

            foreach (var fields in _blueprintLine.BlueprintFields)
            {
                var data = _rawDataColl[fields.Position];

                _converter = Converter.GetConvert(fields.Type);
                _converter.Init(fields, data);

                _data.AddField(fields.Name, _converter.Data, fields.Type);
            }

            return _data;
        }

        public ReadOnlyCollection<IResult> Result
        {
            get
            {
                HasBluprint();
                HasDataToParse();
                return _results.AsReadOnly();    
            }
        }

        public bool IsValid
        {
            get
            {
                HasBluprint();
                HasDataToParse();
                
                return ValidSintaxLine() && ValidSintaxAttribute();
            }
        }

        #endregion

        private int AmountSplitInRawDataCharacter()
        {
            return _rawDataLine.Count(c => c == _blueprintLine.Blueprint.BluePrintCharSepartor);
        }

        private string NormalizeRawData()
        {
            var amoutFields = _blueprintLine.BlueprintFields.Count - 1;

            if (AmountSplitInRawDataCharacter() >= amoutFields)
                return _rawDataLine;

            var splitChar = _blueprintLine.Blueprint.BluePrintCharSepartor;
            var amoutToInsert = amoutFields - AmountSplitInRawDataCharacter();
            
            return String.Concat(_rawDataLine, "".PadRight(amoutToInsert, splitChar));
        }

        private void HasBluprint()
        {
            if (_blueprintLine == null)
                throw new System.Exception("Nenhum BluprintLine configurada para o parser.");
        }

        private void HasDataToParse()
        {
            if (String.IsNullOrEmpty(_rawDataLine))
                throw new System.Exception("Nenhum Dado para ser analisado e importado.");
        }

        private bool ValidSintaxLine()
        {
            _validate = new ValidateLineSeparatedCharacter(_rawDataLine, _blueprintLine);

            if (!_validate.IsValid)
                _results.Add(_validate.Result);

            return _results.Count == 0 || _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }

        private bool ValidSintaxAttribute()
        {
            for (var i = 0; i < _rawDataColl.Length; i++)
            {
                var field = _blueprintLine.BlueprintFields[i];
                var data = _rawDataColl[i];
                _validate = new ValidateField(data, field);

                if (_validate.IsValid) 
                    continue;

                if (_blueprintLine.Occurrence.Type != EnumOccurrence.NoOrMany && _blueprintLine.Occurrence.Type != EnumOccurrence.NoOrOne && _validate.Result.Type == ExceptionType.Warnning)
                {
                    _validate.Result.SetExceptionType(ExceptionType.Error);
                    _validate.Result.SetExceptionSeverity(ExceptionSeverity.Critical);
                }

                _results.Add(_validate.Result);
            }

            return _results.Count == 0 || _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }
    }
}
