using FlatFileImport.Core;

namespace FlatFileImport.Process
{
    public class DefaultParsePolicy : IParsePolicy
    {
        public bool IgnoreLine(IBlueprintLine line)
        {
            return false;
        }


        public bool IgnoreData(Data.IParsedObjetct parsedData)
        {
            return false;
        }
    }
}
