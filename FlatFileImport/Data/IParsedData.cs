using System.Collections.ObjectModel;
using System;

namespace FlatFileImport.Data
{
    public interface IParsedData
    {
        IParsedData Parent { get; }
        string Name { get; }
        ReadOnlyCollection<IParsedData> Datas { get; }
        ReadOnlyCollection<IParsedObjetct> Lines { get; }

        void AddField(string name, string value, Type type);
        void AddParsedData(string name);
        void AddLine(string name);
    }
}
