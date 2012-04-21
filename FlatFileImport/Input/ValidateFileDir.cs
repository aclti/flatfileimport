using System;
using System.IO;
using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Input
{
    class ValidateFileDir : IValidate
    {
        private readonly string _path;

        public ValidateFileDir(string path)
        {
            if(String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            _path = path;
        }

        #region IValidate Members


        public ValidResult ValidateResult
        {
            get { return Valid(); }
        }

        public bool IsValid()
        {
            var path = _path;

            var dir = Path.GetDirectoryName(path);

            if (String.IsNullOrEmpty(dir))
                return false;

            if (Directory.Exists(dir))
                return true;

            if (File.Exists(path))
                return true;

            return false;
        }

        public ValidResult Valid()
        {
            if (IsValid())
                return null;

            return new ValidResult { Message = "Invalid Path", Severity = ExceptionSeverity.Fatal, Type = ExceptionType.Error };
        }

        #endregion
    }
}
