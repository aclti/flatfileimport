using System;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
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

        //TODO: Ser migrar para uma super classe deve ser um método abstrato
        private bool IsValidSintaxLine()
        {
            return _blueprintLine.Regex.IsMatch(_rawDataLine) && _rawDataColl.Length == _blueprintLine.BlueprintFields.Count;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        private bool IsValidSintaxAttribute()
        {
            for (var i = 0; i < _rawDataColl.Length; i++)
            {
                var field = _blueprintLine.BlueprintFields[i];
                var regex = field.Regex;
                var data = _rawDataColl[i];

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
