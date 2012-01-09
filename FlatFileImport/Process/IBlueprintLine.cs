using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public interface IBlueprintLine
    {
        string Class { set; get; }
        Regex Regex { set; get; }
        List<IBlueprintField> BlueprintFields { set; get; }
        bool Mandatory { set; get; }
    }
}
