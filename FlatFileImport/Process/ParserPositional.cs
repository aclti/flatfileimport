using System;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public class ParserPositional : IParser
    {
        private string _rawDataLine;
        private IBlueprintLine _blueprintLine;
        private IParsedObjetct _data;
        private Converter _converter;

        #region IParser Members

        public void SetDataToParse(string rawLine)
        {
            if (String.IsNullOrEmpty(rawLine))
                throw new ArgumentNullException("rawLine");

            _rawDataLine = rawLine;
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

            foreach (var field in _blueprintLine.BlueprintFields)
            {
                var data = _rawDataLine.Substring(field.Position - 1, field.Size);

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

            _data = new ParsedLine(_blueprintLine.Name, parent);

            foreach (var field in _blueprintLine.BlueprintFields)
            {
                var data = _rawDataLine.Substring(field.Position - 1, field.Size);

                _converter = Converter.GetConvert(field.Type);
                _converter.Init(field, data);

                _data.AddField(field.Name, _converter.Data, field.Type);
            }

            return _data;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public ValidResult Result()
        {
            HasBluprint();
            HasDataToParse();
            throw new NotImplementedException();
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public bool IsValid
        {
            get
            {
                HasBluprint();
                HasDataToParse();

                return IsValidSintaxLine() && IsValidSintaxAttribute();
            }
        }

        #endregion

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

        //TODO: Ser migrar para uma super classe deve ser um método abstrato
        private bool IsValidSintaxLine()
        {
            var sum = _blueprintLine.BlueprintFields.Sum(b => b.Size);
            return _rawDataLine.Length == sum && _blueprintLine.Regex.IsMatch(_rawDataLine);
        }

        // TODO: Migrar para um super classe ou para um command pattern
        //TODO: Ser migrar para uma super classe deve ser um método abstrato
        private bool IsValidSintaxAttribute()
        {
            foreach (var field in _blueprintLine.BlueprintFields)
            {
                var regex = field.Regex;
                var data = _rawDataLine.Substring(field.Position - 1, field.Size);

                if (String.IsNullOrEmpty(data))
                    continue;

                if (regex != null && !regex.Rule.IsMatch(data))
                    return false;

                if (field.Type == typeof(string) && data.Length > field.Size)
                    return false;
            }

            return true;
        }
    }
}
