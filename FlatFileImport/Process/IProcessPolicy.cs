using FlatFileImport.Core;
using FlatFileImport.Data;

namespace FlatFileImport.Process
{
    public interface IParsePolicy
    {
        bool IgnoreLine(IBlueprintLine line);
        bool IgnoreData(IParsedObjetct parsedData);
    }
}
