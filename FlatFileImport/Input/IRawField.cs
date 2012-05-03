namespace FlatFileImport.Input
{
    public interface IRawField
    {
        IRawLine Parent { get; }
        string Value { get; }
    }
}
