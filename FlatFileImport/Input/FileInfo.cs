using System;
using System.IO;

namespace FlatFileImport.Input
{
    public class FileInfo : IFileInfo
    {
        private readonly string _path;
        private StreamReader _stream;

	    public FileInfo(string path, FileExtension extension)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (extension == null)
                throw new ArgumentNullException("extension");

            Extesion = extension;
            _path = path;
            
            _stream = new StreamReader(_path);
            Header = _stream.ReadLine();
            ClearAll();
        }

        #region IFileInfo Members

		[Obsolete("Use o Handler para obter essas informações")]
        public string Name { get { return System.IO.Path.GetFileName(Path); } }
		[Obsolete("Use o Handler para obter essas informações")]
        public string Path { get { return  _path; } }
		[Obsolete("Use o Handler para obter essas informações")]
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
	    public FileExtension Extesion { get; set; }

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

			if (_stream == null) 
				return;

			_stream.Close();
			_stream.Dispose();
			_stream = null;
		}

		#endregion
	}
}
