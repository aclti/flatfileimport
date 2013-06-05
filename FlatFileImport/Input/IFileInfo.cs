using System;

namespace FlatFileImport.Input
{
	public interface IFileInfo : IDisposable
	{
		[Obsolete("Use o Handler para obter essas informações")]
		string Name { get; }
		[Obsolete("Use o Handler para obter essas informações")]
		string Path { get; }
		[Obsolete("Use o Handler para obter essas informações")]
		string Directory { get; }

		string Line { get; }
		int LineNumber { get; }
		FileExtension Extesion { get; }
		string Header { get; }
		bool MoveToNext();
		void Reset();
		[Obsolete("Use Dispose")]
		void Release();
	}
}
