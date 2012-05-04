using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Input;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public class ParserSeparatedCharacter : IParser
    {
        private IRawLine _rawLine;
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

        public void SetDataToParse(IRawLine rawLine)
        {
            if (rawLine == null)
                throw new ArgumentNullException("rawLine");

            // FEI PRA CARALHO........ Essa estrutura precisa ser melhorada.
            _rawLine = rawLine;
            _rawLine = new RawLine(_rawLine.Number, NormalizeRawData());

            var aux = _rawLine.Value.Split(_blueprintLine.Blueprint.BluePrintCharSepartor);
            foreach (var field in aux.ToList())
                _rawLine.AddRawFiled(field);
        }

        public void SetBlueprintLine(IBlueprintLine blueprintLine)
        {
            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _results = new List<IResult>();
            _blueprintLine = blueprintLine;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedData(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            var rawFields = _rawLine.RawFields;
            _data = new ParsedData(_blueprintLine.Name, parent);

            foreach (var field in _blueprintLine.BlueprintFields)
            {
                var data = rawFields[field.Position].Value;

                _converter = Converter.GetConvert(field.Type);
                _converter.Init(field, data);

                _data.AddField(field.Name, _converter.Data, field.Type);
            }

            return _data;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedLine(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            var rawFields = _rawLine.RawFields;
            _data = new ParsedLine(_blueprintLine.Name, parent);

            foreach (var field in _blueprintLine.BlueprintFields)
            {
                var data = rawFields[field.Position].Value;

                _converter = Converter.GetConvert(field.Type);
                _converter.Init(field, data);

                _data.AddField(field.Name, _converter.Data, field.Type);
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
            return _rawLine.Value.Count(c => c == _blueprintLine.Blueprint.BluePrintCharSepartor);
        }

        private string NormalizeRawData()
        {
            var amoutFields = _blueprintLine.BlueprintFields.Count - 1;

            if (AmountSplitInRawDataCharacter() >= amoutFields)
                return _rawLine.Value;

            var splitChar = _blueprintLine.Blueprint.BluePrintCharSepartor;
            var amoutToInsert = amoutFields - AmountSplitInRawDataCharacter();

            return String.Concat(_rawLine.Value, "".PadRight(amoutToInsert, splitChar));
        }

        private void HasBluprint()
        {
            if (_blueprintLine == null)
                throw new System.Exception("Nenhum BluprintLine configurada para o parser.");
        }

        private void HasDataToParse()
        {
            if (_rawLine == null)
                throw new System.Exception("Nenhum Dado para ser analisado e importado.");
        }

        private bool ValidSintaxLine()
        {
            _validate = new ValidateLineSeparatedCharacter(_rawLine, _blueprintLine);

            if (!_validate.IsValid)
                _results.Add(_validate.Result);

            return _results.Count == 0;// || _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }

        private bool ValidSintaxAttribute()
        {
            for (var i = 0; i < _rawLine.RawFields.Count; i++)
            {
                var field = _blueprintLine.BlueprintFields[i];
                var data = _rawLine.RawFields[i];
                _validate = new ValidateField(data, field);

                if (_validate.IsValid)
                    continue;

                _results.Add(_validate.Result);
            }

            return _results.Count == 0;// || _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }
    }
}
