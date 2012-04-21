using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public interface IParser
    {
        ParsedLine GetParsedData();
        ValidResult Result();
        bool IsValid { get; }
    }
}
