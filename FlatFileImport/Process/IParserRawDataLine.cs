using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public interface IParserRawDataLine
    {
        IBlueprintLine BlueprintLine { get; }
        string[] RawDataCollection { get; } 
        void ParseRawLineData(string rawDataLine);
        List<ParsedData> ParsedDatas { get; }
    }
}
