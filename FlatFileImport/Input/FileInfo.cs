using System;
using System.IO;

namespace FlatFileImport.Input
{
    public class FileInfo : IFileInfo
    {
        private readonly string _path;
        private StreamReader _stream;
        private FileExtension _extension;
        private readonly FileInfo _parent;

        public FileInfo(string path, FileExtension extension)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (extension == null)
                throw new ArgumentNullException("extension");

            _extension = extension;
            _path = path;
            
            _stream = new StreamReader(_path);
            Header = _stream.ReadLine();
            ClearAll();
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

        public string Header { get; private set; }
        public string Line { get; private set; }
        public int LineNumber { get; private set; }

        public bool MoveToNext()
        {
            if (_stream == null)
                _stream = new StreamReader(_path);

            if (_stream.EndOfStream)
                return false;

            LineNumber++;
            Line = _stream.ReadLine();
            return true;
        }

        public void Reset()
        {
            Clear();

            if (_stream == null) 
                return;

            _stream.DiscardBufferedData();
            _stream.BaseStream.Seek(0, SeekOrigin.Begin);
        }

		[Obsolete("Use Dispose")]
        public void Release()
        {
            Dispose();
        }

        #endregion

        private void ClearAll()
        {
            Clear();

            if(_stream == null)
                return;

            _stream.Close();
            _stream.Dispose();
            _stream = null;
        }

        private void Clear()
        {
            LineNumber = 0;
            Line       = String.Empty;
        }

		#region IDisposable Members

		public void Dispose()
		{
			Clear();

			if (_stream != null)
			{
				_stream.Close();
				_stream.Dispose();
				_stream = null;	
			}

			if (_parent == null)
				return;

			if (Extesion.Type != FileType.Binary)
				return;

			File.Delete(_path);
			var dir = System.IO.Path.GetDirectoryName(_path);
			System.IO.Directory.Delete(dir);
			_parent.Dispose();
		}

		#endregion
	}
}
