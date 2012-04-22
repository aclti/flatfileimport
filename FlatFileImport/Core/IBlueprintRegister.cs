﻿using System.Text.RegularExpressions;

namespace FlatFileImport.Core
{
    public interface IBlueprintRegister
    {
        IBlueprint Blueprint { get; }
        string Class { set; get; }
        Regex End { set; get; }
        Regex Begin { set; get; }
        bool IsComplet { set; get; }
    }
}