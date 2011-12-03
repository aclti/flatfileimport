using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FlatFileImport.Exception;
using FlatFileImport.Validate;

namespace FlatFileImport.Input
{
    public abstract class Handler : IEnumerable<FileInfo>, IEnumerator<FileInfo>
    {
        private string _path;
        private int _pos;
        protected List<FileInfo> FileInfos;

        public List<string> SupportedExtesion { get { return GetSupportedExtesion(); } }
        public IValidate Validate { get; private set; }
        public string Path
        {
            get { return _path; }
            set { if (!Validate.IsValid())throw new FileNotFoundException(); _path = value; }
        }

        protected Handler(string path)
        {
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

        private static bool IsPlainText(string path)
        {
            var l = GetSupportedExtesion(FileType.Text);
            return l.Contains(System.IO.Path.GetExtension(path));
        }

        private static bool IsZipFile(string path)
        {
            var l = GetSupportedExtesion(FileType.Binary);
            return l.Contains(System.IO.Path.GetExtension(path));
        }

        private static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        private static List<string> GetSupportedExtesion(FileType type)
        {
            var l = new List<string>();
            var confPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config-input.xml");

            var conf = new XmlDocument();
            conf.Load(confPath);

            var nodes = conf.SelectNodes("//Configuration/Extension[@type='" + type.ToString().ToLower() + "']");

            if (nodes != null)
                l.AddRange(from XmlNode n in nodes select n.InnerText);

            return l;
        }

        private static List<string> GetSupportedExtesion()
        {
            var l = new List<string>();
            var confPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config-input.xml");

            var conf = new XmlDocument();
            conf.Load(confPath);

            var nodes = conf.SelectNodes("//Configuration/Extension");

            if (nodes != null)
                l.AddRange(from XmlNode n in nodes select n.InnerText);

            return l;
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
