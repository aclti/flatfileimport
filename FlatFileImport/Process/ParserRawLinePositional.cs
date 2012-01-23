using System;
using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public class ParserRawLinePositional : IParserRawDataLine
    {
        //private string _rawLine;
        //private BlueprintLine _blueprintLine;

        #region IParserRawDataLine Members

        public string[] RawDataCollection
        {
            get { throw new NotImplementedException(); }
        }

        public void ParseRawLineData(string rawDataLine, IBlueprintLine blueprintLine)
        {
            throw new NotImplementedException();
        }

        public List<ParsedData> ParsedDatas
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
