using System.Collections.ObjectModel;

namespace FlatFileImport.Input
{
    public interface IRawLine
    {
        int Number { get; }
        string Value { get; }
        ReadOnlyCollection<IRawField> RawFields { get; }

        void AddRawFiled(string rawValue);
    }
}