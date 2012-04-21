using System;
using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Log 
{
    public class DefaultEventLog : IEventLog
    {
        #region IEventLog Members

        public string GetShotMessage()
        {
            throw new NotImplementedException();
        }

        public string GetMessage()
        {
            throw new NotImplementedException();
        }

        public string GetFullMessage()
        {
            throw new NotImplementedException();
        }

        public bool SetMessage(ValidResult result)
        {
            throw new NotImplementedException();
        }

        public bool SetMessage(string message)
        {
            throw new NotImplementedException();
        }

        public bool SetMessage(System.Exception exception)
        {
            throw new NotImplementedException();
        }

        public bool SetMessage(IImporterException exception)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
