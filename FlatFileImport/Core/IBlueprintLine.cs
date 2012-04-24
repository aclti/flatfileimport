using System.Collections.Generic;
using System.Text.RegularExpressions;
using FlatFileImport.Aggregate;

namespace FlatFileImport.Core
{
    public interface IBlueprintLine
    {
        IBlueprint Blueprint { get; }
        IBlueprintLine Parent { get; }
        string Name { set; get; }
        Regex Regex { set; get; }
        List<IBlueprintField> BlueprintFields { set; get; }
        IOccurrence Occurrence { set; get; }
        IAggregate Aggregate { set; get; }
    }
}
