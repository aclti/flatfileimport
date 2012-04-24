using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport.Core
{
    public interface IBlueprintFactoy
    {
        IBlueprint GetBlueprint(FileInfo toParse);
        IBlueprint GetBlueprint(object selectParam);
        IBlueprint GetBlueprint(object selectParam, FileInfo toParse);
    }
}
