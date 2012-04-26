using FlatFileImport.Data;

namespace FlatFileImport
{
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void UnRegisterObserver(IObserver observer);
        void NotifyObservers(IParsedData data);
        void NotifyObservers(IParsedObjetct data);
        void NotifyObservers(IParsedData[] data);
        void NotifyObservers(string[] data);
    }
}
