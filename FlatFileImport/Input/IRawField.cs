namespace FlatFileImport.Input
{
    public interface IRawField
    {
        IRawLineAndFields Parent { get; }
        string Value { get; }
    }
}
