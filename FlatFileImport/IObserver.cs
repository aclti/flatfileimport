using System.Collections.Generic;
using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport
{
    public interface IObserver
    {
        void Notify(IParsedData[] data);
        void Notify(List<IResult> data);
    }
}

