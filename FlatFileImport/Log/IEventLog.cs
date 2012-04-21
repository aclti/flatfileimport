using FlatFileImport.Validate;
using FlatFileImport.Exception;

namespace FlatFileImport.Log
{
    public interface IEventLog
    {
        string GetShotMessage();
        string GetMessage();
        string GetFullMessage();
        bool SetMessage(ValidResult result);
        bool SetMessage(string message);
        bool SetMessage(System.Exception exception);
        bool SetMessage(IImporterException exception);
    }
}
