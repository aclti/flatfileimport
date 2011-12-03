using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintLine
    {
        public string Class { set; get; }
        public Regex Regex { set; get; }
        public List<BlueprintField> BlueprintFields { set; get; }
        public bool Mandatory { set; get; }
    }
}
