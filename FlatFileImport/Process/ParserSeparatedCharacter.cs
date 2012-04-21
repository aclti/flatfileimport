using System;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public class ParserSeparatedCharacter : IParser
    {
        private string _rawDataLine;
        private string[] _rawDataColl;
        private IBlueprintLine _blueprintLine;
        private ParsedData _data;
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
        public ParsedData GetParsedData()
        {
            _data = new ParsedData(_blueprintLine.Class, _blueprintLine.Mandatory);

            foreach (var fields in _blueprintLine.BlueprintFields)
            {
                var data = _rawDataColl[fields.Position];

                _converter = Converter.GetConvert(fields.Type);
                _converter.Init(fields, data);

                _data.AddAtributte(fields.Attribute, _converter.Data, fields.Type);
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
