using System;
using System.IO;

namespace FlatFileImport.Input
{
    public class FileInfo : IFileInfo
    {
        private readonly string _path;
        private StreamReader _stream;
        private string _header;
        private FileExtension _extension;
        private readonly FileInfo _parent;
        private string _line;
        private int _lineNumber;

        public FileInfo(string path, FileExtension extension)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (extension == null)
                throw new ArgumentNullException("extension");

            _extension = extension;
            _path = path;
        }

        public FileInfo(string path, FileExtension extension, FileInfo parent) : this(path, extension)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;
        }

        #region IFileInfo Members

        public string Name { get { return System.IO.Path.GetFileName(Path); } }
        public string Path { get { return _parent == null ? _path : _parent.Path; } }
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
        public FileExtension Extesion { get { return _parent == null ? _extension : _parent.Extesion; } set { _extension = value; } }

        public string Header
        {
            get
            {
                if (String.IsNullOrEmpty(_header))
                {
                    MoveToNext();
                    _header = Line;
                }

                return _header;
            }
        }

        public string Line
        {
            get { return _lineNumber <= 0 ? Header : _line; }
        }

        public bool MoveToNext()
        {
            if (_stream == null)
                _stream = new StreamReader(_path);

            if (_stream.EndOfStream)
                return false;

            _lineNumber++;
            _line = _stream.ReadLine();
            return true;
        }

        public int LineNumber
        {
            get { return _lineNumber; }
        }

        #endregion
    }
}
