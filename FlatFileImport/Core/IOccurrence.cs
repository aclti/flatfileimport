namespace FlatFileImport.Core
{
    public interface IOccurrence
    {
        IBlueprintLine BlueprintLine { get; }
        int? Min { get; }
        int? Max { get; }
        EnumOccurrence Type { get; }
    }
}
