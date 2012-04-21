using System;
using FlatFileImport.Exception;

namespace FlatFileImport.Log
{
    public class NotLogEvent : IEventLog
    {
        #region IEventLog Members

        public string GetShotMessage()
        {
            return String.Empty;
        }

        public string GetMessage()
        {
            return String.Empty;
        }

        public string GetFullMessage()
        {
            return String.Empty;
        }

        public bool SetMessage(Validate.ValidResult result)
        {
            return true;
        }

        public bool SetMessage(string message)
        {
            return true;
        }

        public bool SetMessage(System.Exception exception)
        {
            return true;
        }

        public bool SetMessage(IImporterException exception)
        {
            return true;
        }

        #endregion
    }
}
