using System;
using System.IO;
using Ionic.Zip;
using System.Linq;

namespace FlatFileImport.Input
{
    // TODO: Fazer com que o handler consiga trabalhar com zip que contenham mais de um arquivo ou diretorio
    public class HandlerZip : Handler
    {
        private string _dataFile;
	    private readonly IHandler _innerHandler;
	    private readonly IHandlerFactory _factory;
	    private readonly string _path;

		public HandlerZip(string path, ISupportedExtension supportedExtension, IHandlerFactory factory) : base(path, supportedExtension)
		{
			_factory = factory;
			_path = path;

			_innerHandler = _factory.Get(ExtractZip(_path));
		}

        private string ExtractZip(string path)
        {
            using (var zip = ZipFile.Read(path))
            {
                if (zip.Entries.Count > 1)
                    throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL (Varios arquivos no Zip) | " + path);

                var entry = zip.Entries.FirstOrDefault();

                if (entry == null)
                    return null;

                if (entry.IsDirectory)
                    throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL (Zip com diretório) | " + path);

	            var tempExtractDir = System.IO.Directory.CreateDirectory(String.Format("{0}FLAT-FILE-EXTRACT-{1}", System.IO.Path.GetTempPath(), DateTime.Now.Ticks));

				entry.Extract(tempExtractDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                _dataFile = entry.FileName;

                zip.Dispose();
				return System.IO.Path.Combine(tempExtractDir.FullName, _dataFile);
            }
        }

		public override IFileInfo FileInfo
		{
			get { return _innerHandler.FileInfo; }
		}

		public override string Path
		{
			get { return _path; }
		}

		public override void Dispose()
		{
			_innerHandler.Dispose();

			File.Delete(_innerHandler.Path);
			var dir = System.IO.Path.GetDirectoryName(_innerHandler.Path);
			
			if (string.IsNullOrEmpty(dir))
				return;

			Directory.Delete(dir);
		}
	}
}
