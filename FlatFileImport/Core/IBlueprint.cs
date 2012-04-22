using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Core
{
    public interface IBlueprint
    {
        IBlueprintLine Header { get; }
        IBlueprintLine Footer { get; }
        ReadOnlyCollection<IBlueprintRegister> BlueprintRegistires { get; }
        ReadOnlyCollection<IBlueprintLine> BlueprintLines { get; }
        EnumFieldSeparationType FieldSeparationType { get; }
        char BluePrintCharSepartor { get; }
        bool UseRegistries { get; }

        void AddBlueprintLines(IBlueprintLine blueprintLine);
        void AddBlueprintLines(List<IBlueprintLine> blueprintLines);
    }
}
