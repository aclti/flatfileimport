using System;
using System.IO;

namespace FlatFileImport.Input
{
	public abstract class Handler : IHandler
	{
		private readonly string _path;
		//private int _pos;

		protected ISupportedExtension SupportedExtension;

		#region IHandler Members

		public abstract IFileInfo FileInfo { get; }
		public abstract string Path { get; }

		#endregion

		protected Handler(string path, ISupportedExtension supportedExtension)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");

			_path = path;

			if (!IsValid())
				throw new ArgumentException("O Path informado não é válido.");

			SupportedExtension = supportedExtension;
		}

		private bool IsValid()
		{

			var fInfo = new System.IO.FileInfo(_path);

			if (fInfo.Exists)
				return true;

			var dInfo = new DirectoryInfo(_path);

			return dInfo.Exists;
		}

		#region IDisposable Members

		public abstract void Dispose();

		#endregion

		//#region IEnumerable<IFileInfo> Members

		//[Obsolete]
		//public IEnumerator<IFileInfo> GetEnumerator()
		//{
		//	return FileInfos.GetEnumerator();
		//}

		//#endregion

		//#region IEnumerable Members

		//[Obsolete]	
		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	return FileInfos.GetEnumerator();
		//}

		//#endregion

		//#region IEnumerator<IFileInfo> Members

		//public IFileInfo Current
		//{
		//	get { return FileInfos[_pos]; }
		//}

		//#endregion

		//#region IDisposable Members

		//public void Dispose()
		//{
		//	throw new NotImplementedException();
		//}

		//#endregion

		//#region IEnumerator Members

		//object IEnumerator.Current
		//{
		//	get { return FileInfos[_pos]; }
		//}

		//public bool MoveNext()
		//{
		//	if (_pos >= FileInfos.Count - 1)
		//		return false;

		//	++_pos;
		//	return true;
		//}

		//public void Reset()
		//{
		//	_pos = 0;
		//}

		//#endregion
	}
}
