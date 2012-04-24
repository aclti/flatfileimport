using System.Collections.ObjectModel;
using System;

namespace FlatFileImport.Data
{
    public interface IParsedObjetct
    {
        IParsedData Parent { get; }
        string Name { get; }
        ReadOnlyCollection<IParsedField> Fields { get; }
        void AddField(string name, string value, Type type);
   }
}
