using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public interface IParser
    {
        IParsedObjetct GetParsedLine(IParsedData parent);
        IParsedObjetct GetParsedData(IParsedData parent);
        void SetBlueprintLine(IBlueprintLine blueprintLine);
        void SetDataToParse(string rawLine);
        ValidResult Result();
        bool IsValid { get; }
    }
}
