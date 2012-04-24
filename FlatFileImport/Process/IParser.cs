using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public interface IParser
    {
        IParsedObjetct GetParsedLine(IParsedData parent);
        IParsedData GetParsedData(IParsedData parent);
        ValidResult Result();
        bool IsValid { get; }
    }
}
