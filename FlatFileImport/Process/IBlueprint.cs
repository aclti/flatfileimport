using System.Collections.Generic;

namespace FlatFileImport.Process
{
    public interface IBlueprint
    {
        BlueprintLine Header { get; }
        BlueprintLine Footer { get; }
        List<BlueprintRegister> BlueprintRegistires { get; }
        List<BlueprintLine> BlueprintLines { get; }
        EnumFieldSeparationType FieldSeparationType { get; }
        char BluePrintCharSepartor { get; }
        bool UseRegistries { get; }
    }
}
