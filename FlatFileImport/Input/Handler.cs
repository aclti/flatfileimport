using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FlatFileImport.Exception;

namespace FlatFileImport.Input
{
    public abstract class Handler : IEnumerable<FileInfo>, IEnumerator<FileInfo>
    {
        protected static SupportedExtension SupportedExtension;
        private readonly string _path;
        private int _pos;
        protected List<FileInfo> FileInfos;

        public static ReadOnlyCollection<FileExtension> Extensions { get { return SupportedExtension.Extensions; } }
        public string Path { get { return _path; } }

        static Handler()
        {
            if (SupportedExtension == null)
                SupportedExtension = new SupportedExtension();
        }

        protected Handler(string path)
        {
            if(String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (SupportedExtension == null)
                SupportedExtension = new SupportedExtension();

            _path = path;

            if (!IsValid())
                throw new ArgumentException("O Path informado não é válido.");

            FileInfos = new List<FileInfo>();
        }

        public static IEnumerable<FileInfo> GetHandler(string path)
        {
            if (IsDirectory(path))
                return new HandlerDirectory(path);

            if (IsPlainText(path))
                return new HandlerText(path);

            if (IsZipFile(path))
                return new HandlerZip(path);

            throw new WrongTypeFileException(path);
        }

        public static void AddExtension(string extension, FileType type)
        {
            SupportedExtension.AddExtension(extension, type);
        }

        public static void AddExtensionFromXml(string path)
        {
            SupportedExtension.AddExtensionFromXml(path);
        }

        private static bool IsPlainText(string path)
        {
            var ex = SupportedExtension.GetFileExtension(path);
            return ex != null && Extensions.Any(e => e.Name == ex.Name && ex.Type == FileType.Text);
        }

        private static bool IsZipFile(string path)
        {
            var ex = SupportedExtension.GetFileExtension(path);
            return ex != null && Extensions.Any(e => e.Name == ex.Name && ex.Type == FileType.Binary && e.Name == ".zip");
        }

        private static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        private bool IsValid()
        {

            var fInfo = new System.IO.FileInfo(_path);

            if (fInfo.Exists)
                return true;

            var dInfo = new DirectoryInfo(_path);

            return dInfo.Exists;
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
