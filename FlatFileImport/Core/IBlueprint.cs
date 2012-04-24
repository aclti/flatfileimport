using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Core
{
    public interface IBlueprint
    {
        ReadOnlyCollection<IBlueprintLine> BlueprintLines { get; }
        EnumFieldSeparationType FieldSeparationType { get; }
        char BluePrintCharSepartor { get; }

        void AddBlueprintLines(IBlueprintLine blueprintLine);
        void AddBlueprintLines(List<IBlueprintLine> blueprintLines);
    }
}
