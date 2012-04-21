using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public interface IParser
    {
        ParsedData GetParsedData();
        ValidResult Result();
        bool IsValid { get; }
    }
}
