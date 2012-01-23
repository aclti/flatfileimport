using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public interface IParserRawDataLine
    {
        string[] RawDataCollection { get; } 
        void ParseRawLineData(string rawDataLine, IBlueprintLine blueprintLine);
        List<ParsedData> ParsedDatas { get; }
    }
}
