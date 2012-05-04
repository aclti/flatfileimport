using System.Collections.Generic;
using FlatFileImport.Data;
using FlatFileImport.Validate;

namespace FlatFileImport
{
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void UnRegisterObserver(IObserver observer);
        void NotifyObservers(IParsedData[] data);
        void NotifyObservers(List<IResult> data);
    }
}

