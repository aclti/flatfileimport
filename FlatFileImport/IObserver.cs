using System.Collections.Generic;
using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport
{
    public interface IObserver
    {
        void Notify(IParsedData data);
        void Notify(IParsedObjetct data);
        void Notify(IParsedData[] data);
        void Notify(string[] data);
        void Notify(List<IResult> data);
    }
}

