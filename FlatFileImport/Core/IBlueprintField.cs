using System;
using FlatFileImport.Process;

namespace FlatFileImport.Core
{
    public interface IBlueprintField
    {
        IBlueprintLine Parent { get; }
        string Name { set; get; }
        Type Type { set; get; }
        int Size { set; get; }
        int Precision { set; get; }
        RegexRule Regex { set; get; }
        bool Persist { set; get; }
        int Position { get; set; }
    }
}