using System;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintField : IBlueprintField
    {
        public string Class { set; get; }
        public string Attribute { set; get; }
        public Type Type { set; get; }
        public int Size { set; get; }
        public int Precision { set; get; }
        public Regex Regex { set; get; }
        public bool Persist { set; get; }
        public int Position { get; set; }
    }
}
