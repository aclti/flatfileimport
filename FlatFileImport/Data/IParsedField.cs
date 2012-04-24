using System;

namespace FlatFileImport.Data
{
    public interface IParsedField
    {
        IParsedObjetct Parent { get; }
        string Name { get; }
        Type Type { get; }
        string Value { get; }
    }
}
