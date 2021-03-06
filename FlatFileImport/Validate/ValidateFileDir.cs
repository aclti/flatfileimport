﻿using System;
using System.IO;
using FlatFileImport.Exception;

namespace FlatFileImport.Validate
{
    public class ValidateFileDir : IValidate
    {
        private readonly string _path;

        public ValidateFileDir(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            _path = path;
        }

        #region IValidate Members

        bool IValidate.IsValid
        {
            get
            {
                var path = _path;

                var dir = Path.GetDirectoryName(path);

                if (String.IsNullOrEmpty(dir))
                {
                    Result = new Result("Invalid Directory", ExceptionType.Error, ExceptionSeverity.Fatal) { Value = dir };
                    return false;
                }

                if (Directory.Exists(dir))
                    return true;

                if (File.Exists(path))
                    return true;

                Result = new Result("Invalid File Path", ExceptionType.Error, ExceptionSeverity.Fatal) { Value = path };
                return false;
            }
        }

        public IResult Result { get; private set; }

        #endregion
    }
}
