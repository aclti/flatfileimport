using FlatFileImport.Input;

namespace FlatFileImport.Core
{
    public interface IBlueprintFactoy
    {
        IBlueprint GetBlueprint(IFileInfo toParse);
        IBlueprint GetBlueprint(object selectParam, IFileInfo toParse);
    }
}
