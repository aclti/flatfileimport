using System;

namespace FlatFileImport.Input
{
    public interface IFileInfo : IDisposable
    {
        string Name { get; }
        string Path { get; }
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
