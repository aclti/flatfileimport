namespace FlatFileImport.Input
{
    public interface IFileInfo
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
        void Release();
    }
}
