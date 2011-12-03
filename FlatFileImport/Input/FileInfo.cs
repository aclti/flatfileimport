﻿using System;
using System.IO;
using FlatFileImport.Validate;

namespace FlatFileImport.Input
{
    public class FileInfo
    {
        private readonly IValidate _validate;
        private readonly string _path;
        private StreamReader _stream;
        private string _header;

        public string Name { get { return System.IO.Path.GetFileName(Path); } }
        public string Path { get { return _path; } }
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
        public string Extesion { get { return System.IO.Path.GetExtension(Path); } }
        public string Comment { set; get; }
        public IValidate Validate { get { return _validate; } }

        public StreamReader Stream { get { return _stream ?? (_stream = new StreamReader(_path)); } }

        public string Header
        {
            get
            {
                if (String.IsNullOrEmpty(_header))
                    _header = Stream.ReadLine();

                return _header;
            }

        }

        public FileInfo(string path)
        {
            _path = path;
            _validate = new ValidateFileDir(path);

            if (!Validate.IsValid())
                throw new ArgumentException("O Path informado não é válido.");
        }
    }
}