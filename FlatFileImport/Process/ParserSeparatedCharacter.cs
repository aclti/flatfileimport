using System;
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

        public ParserSeparatedCharacter(IBlueprintLine blueprintLine, string rawDataLine)
        {
            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            if (String.IsNullOrEmpty(rawDataLine))
                throw new ArgumentNullException("rawDataLine");

            _rawDataLine = rawDataLine;
            _blueprintLine = blueprintLine;
            _rawDataColl = _rawDataLine.Split(_blueprintLine.Blueprint.BluePrintCharSepartor);
        }

        #region IParser Members

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedData GetParsedData(IParsedData parent)
        {
            return new ParsedData("dummy", parent);
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedLine(IParsedData parent)
        {
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
            throw new NotImplementedException();
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public bool IsValid { get { return IsValidSintaxLine() && IsValidSintaxAttribute(); } }

        #endregion

        //TODO: Ser migrar para uma super classe deve ser um método abstrato
        private bool IsValidSintaxLine()
        {
            return  _blueprintLine.Regex.IsMatch(_rawDataLine) && _rawDataColl.Length == _blueprintLine.BlueprintFields.Count;
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
