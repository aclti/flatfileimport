
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

using FlatFileImport.Exception;

namespace FlatFileImport.Input
{
	public class HandlerFacotry : IHandlerFactory
	{
		private readonly ISupportedExtension _supportedExtension;

		public HandlerFacotry(ISupportedExtension extensions)
		{
			_supportedExtension = extensions;
		}

		public string[] IgnoreExtensions { get; set; }
		public ReadOnlyCollection<FileExtension> Extensions { get { return _supportedExtension.Extensions; } }

		public void AddExtension(string extension, FileType type)
		{
			_supportedExtension.AddExtension(extension, type);
		}

		public void AddExtensionFromXml(string path)
		{
			_supportedExtension.AddExtensionFromXml(path);
		}

		private bool IsPlainText(string path)
		{
			var ex = _supportedExtension.GetFileExtension(path);
			return ex != null && Extensions.Any(e => e.Name == ex.Name && ex.Type == FileType.Text);
		}

		private bool IsZipFile(string path)
		{
			var ex = _supportedExtension.GetFileExtension(path);
			return ex != null && Extensions.Any(e => e.Name == ex.Name && ex.Type == FileType.Binary && e.Name == ".zip");
		}

		private bool IsDirectory(string path)
		{
			return Directory.Exists(path);
		}

		#region IHandlerFactory Members

		public IHandler Get(string path)
		{
			if (IsDirectory(path))
				throw new System.Exception("Não pode ser um direotrio");

			if (IgnoreExtensions != null && IgnoreExtensions.Any(e => e.ToUpper() == Path.GetExtension(path).ToUpper()))
				return new HandlerDummy(path, _supportedExtension);

			if (IsPlainText(path))
				return new HandlerText(path, _supportedExtension);

			if (IsZipFile(path))
				return new HandlerZip(path, _supportedExtension, this);

			throw new WrongTypeFileException(path);
		}

		#endregion
	}
}
