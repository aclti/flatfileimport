using System;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public interface IBlueprintField
    {
        string Class { set; get; }
        string Attribute { set; get; }
        Type Type { set; get; }
        int Size { set; get; }
        int Precision { set; get; }
        Regex Regex { set; get; }
        bool Persist { set; get; }
        int Position { get; set; }
    }
}
