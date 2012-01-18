using System;
using System.IO;
using FlatFileImport.Validate;

namespace FlatFileImport.Input
{
    public class FileInfo
    {
        private readonly string _path;
        private StreamReader _stream;
        private string _header;

        public string Name { get { return System.IO.Path.GetFileName(Path); } }
        public string Path { get { return _path; } }
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
        //public FileExtension Extesion { get { return System.IO.Path.GetExtension(Path); } }
        //public FileExtension Extesion { get { return new FileExtension(System.IO.Path.GetExtension(Path), FileType.Text); } }
        //public FileExtension Extesion { get; private set; }
        public string Comment { set; get; }

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
        }
    }
}
