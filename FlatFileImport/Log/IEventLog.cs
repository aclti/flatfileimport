using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Log
{
    public interface IEventLog
    {
        string GetShotMessage();
        string GetMessage();
        string GetFullMessage();
        bool SetMessage(IResult result);
        bool SetMessage(string message);
        bool SetMessage(System.Exception exception);
        bool SetMessage(IImporterException exception);
    }
}
