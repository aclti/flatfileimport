using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintLine : IBlueprintLine
    {
        public string Class { set; get; }
        public Regex Regex { set; get; }
        public List<IBlueprintField> BlueprintFields { set; get; }
        public bool Mandatory { set; get; }
    }
}