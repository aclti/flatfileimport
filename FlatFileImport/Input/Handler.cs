using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Input
{
    public abstract class Handler : IEnumerable<FileInfo>, IEnumerator<FileInfo>
    {
        private static SupportedExtension _suporttedExtesions;
        private string _path;
        private int _pos;
        protected List<FileInfo> FileInfos;

        public static ReadOnlyCollection<FileExtension> SupportedExtesion { get { return _suporttedExtesions.Extension; } }
        public IValidate Validate { get; private set; }
        public string Path
        {
            get { return _path; }
            set { if (!Validate.IsValid())throw new FileNotFoundException(); _path = value; }
        }

        static Handler()
        {
            if(_suporttedExtesions == null)
                _suporttedExtesions = new SupportedExtension();
        }

        protected Handler(string path)
        {
            _suporttedExtesions = new SupportedExtension();
            Validate = new ValidateFileDir(path);

            if (!Validate.IsValid())
                throw new ArgumentException("O Path informado não é válido.");

            _path = path;
            FileInfos = new List<FileInfo>();
        }

        public static IEnumerable<FileInfo> GetHandler(string path)
        {
            if(IsDirectory(path))
                return new HandlerDirectory(path);

            if (IsPlainText(path))
                return new HandlerText(path);

            if (IsZipFile(path))
                return new HandlerZip(path);

            throw new WrongTypeFileException(path);
        }

        public static void AddExtension(string extension, FileType type)
        {
            _suporttedExtesions.AddExtension(extension, type);
        }

        public static void AddExtensionFromXml(string path)
        {
            _suporttedExtesions.AddExtensionFromXml(path);
        }

        private static bool IsPlainText(string path)
        {
            return _suporttedExtesions.IsSupported(System.IO.Path.GetExtension(path));
        }

        private static bool IsZipFile(string path)
        {
            return _suporttedExtesions.IsSupported(System.IO.Path.GetExtension(path));
        }

        private static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        #region IEnumerable<FileInfo> Members

        public IEnumerator<FileInfo> GetEnumerator()
        {
            return FileInfos.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return FileInfos.GetEnumerator();
        }

        #endregion

        #region IEnumerator<FileInfo> Members

        public FileInfo Current
        {
            get { return FileInfos[_pos]; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return FileInfos[_pos]; }
        }

        public bool MoveNext()
        {
            if (_pos >= FileInfos.Count - 1)
                return false;

            ++_pos;
            return true;
        }

        public void Reset()
        {
            _pos = 0;
        }

        #endregion
    }
}
