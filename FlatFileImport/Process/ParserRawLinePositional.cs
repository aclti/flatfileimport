using System;
using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public class ParserRawLinePositional : IParserRawDataLine
    {
        //private string _rawLine;
        //private BlueprintLine _blueprintLine;
        
        private string _rawDataLine;
        private IBlueprintLine _blueprintLine;

        #region IParserRawDataLine Members

        public string[] RawDataCollection { get { return GetRawDataCollection(); } }

        public void ParseRawLineData(string rawDataLine, IBlueprintLine blueprintLine)
        {
            if(String.IsNullOrEmpty(rawDataLine))
                throw new ArgumentNullException("rawDataLine");

            if(blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _rawDataLine = rawDataLine;
            _blueprintLine = blueprintLine;
        }

        public List<ParsedData> ParsedDatas
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


        private string[] GetRawDataCollection()
        {
            var aux = new string[_blueprintLine.BlueprintFields.Count];

            for (var i = 0; i <_blueprintLine.BlueprintFields.Count; i++)
            {
                var bField = _blueprintLine.BlueprintFields[i];
                aux[i] = _rawDataLine.Substring(bField.Position - 1, bField.Size);
            }

            return aux;
        }
    }
}
