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
        private ParsedLine _data;
        private Converter _converter;

        public ParserPositional(IBlueprintLine blueprintLine, string rawDataLine)
        {
            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            if (String.IsNullOrEmpty(rawDataLine))
                throw new ArgumentNullException("rawDataLine");

            _rawDataLine = rawDataLine;
            _blueprintLine = blueprintLine;
        }

        #region IParser Members

        // TODO: Migrar para um super classe ou para um command pattern
        public ParsedLine GetParsedData()
        {
            _data = new ParsedLine(_blueprintLine.Name, /*_blueprintLine.Mandatory*/true);

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
            throw new NotImplementedException();
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public bool IsValid
        {
            get { return IsValidSintaxLine() && IsValidSintaxAttribute(); }
        }

        #endregion

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
