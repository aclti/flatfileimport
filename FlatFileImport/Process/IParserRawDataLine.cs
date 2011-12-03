﻿using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public interface IParserRawDataLine
    {
        string[] RawDataCollection { get; } 
        void ParseRawLineData(string rawDataLine, BlueprintLine blueprintLine);
        List<ParsedData> ParsedDatas { get; }
    }
}