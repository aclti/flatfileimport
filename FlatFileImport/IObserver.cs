using FlatFileImport.Data;

namespace FlatFileImport
{
    public interface IObserver
    {
        void Notify(IParsedData data);
        void Notify(IParsedObjetct data);
        void Notify(IParsedData[] data);
        void Notify(string[] data);
    }
}
