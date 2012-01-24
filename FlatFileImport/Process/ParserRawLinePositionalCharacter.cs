using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public class ParserRawLinePositionalCharacter : IParserRawDataLine
    {
        private string _rawDataLine;
        private IBlueprintLine _blueprintLine;
        private readonly char _separator;

        #region IParserRawDataLine Members

        public string[] RawDataCollection { get { return GetRawData(); } }

        public void ParseRawLineData(string rawDataLine, IBlueprintLine blueprintLine)
        {
            _rawDataLine = rawDataLine;
            _blueprintLine = blueprintLine;
        }

        public List<ParsedData> ParsedDatas { get { return GetParseData(RawDataCollection); } }

        #endregion

        public ParserRawLinePositionalCharacter(char separator)
        {
            _separator = separator;
        }

        private string[] GetRawData()
        {
            // TODO: Isso nunca deve acontecer, verificar a melhor maneira de tratar com exceção
            if (_blueprintLine == null)
                return null;

            string[] data = _rawDataLine.Split(_separator);

            return data;
        }

        private List<ParsedData> GetParseData(string[] data)
        {
            var l = new List<ParsedData>();

            for (int i = 0; i < _blueprintLine.BlueprintFields.Count; i++)
            {
                var parsed = ParsedDataFactory.GetParsedData(data[i], _blueprintLine.BlueprintFields[i]);
                l.Add(parsed);
            }

            return l;

        }
    }
}
