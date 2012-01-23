using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public interface IBlueprint
    {
        IBlueprintLine Header { get; }
        IBlueprintLine Footer { get; }
        List<IBlueprintRegister> BlueprintRegistires { get; }
        List<IBlueprintLine> BlueprintLines { get; }
        EnumFieldSeparationType FieldSeparationType { get; }
        char BluePrintCharSepartor { get; }
        bool UseRegistries { get; }
    }
}
