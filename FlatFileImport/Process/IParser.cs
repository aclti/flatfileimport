using System.Collections.ObjectModel;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Input;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public interface IParser
    {
        IParsedObjetct GetParsedLine(IParsedData parent);
        IParsedObjetct GetParsedData(IParsedData parent);
        void SetBlueprintLine(IBlueprintLine blueprintLine);
        void SetDataToParse(IRawLine rawLine);
        ReadOnlyCollection<IResult> Result { get; }
        bool IsValid { get; }
    }
}
