using System.Collections.ObjectModel;
using System;

namespace FlatFileImport.Data
{
    public interface IParsedData
    {
        IParsedData Parent { get; }
        string Name { get; }
        ReadOnlyCollection<IParsedData> Headers { get; }
        ReadOnlyCollection<IParsedObjetct> Details { get; }
        ReadOnlyCollection<IParsedField> Fields { get; }

        void AddField(string name, string value, Type type);
        void AddParsedData(string name);
        void AddParsedData(IParsedData header);
        void AddLine(string name);
        void AddLine(IParsedObjetct details);
    }
}
