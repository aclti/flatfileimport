namespace FlatFileImport.Core
{
    public enum EnumLineAttributes
    {
        Name, 
        Regex, 
        Occurrence,
        Type,
        Parent,
        Aggregate
    }

    public enum EnumFieldAttributes
    {
        Name,
        Position,
        Type,
        Size,
        Persist,
        Regex,
        Precision,
        Aggregate
    }

    public enum EnumLineType
    {
        None = 0,
        Header,
        Footer,
        Details
    }

    public enum EnumOccurrence
    {
        None = 0,
        One,
        NoOrOne,
        AtLeastOne,
        NoOrMany,
        Range
    }

    public enum EnumFieldSeparationType
    {
        None = 0,
        Character,
        Position
    }

    public enum EnumConfigurationItem
    {
        None = 0,
        FieldSeparationType,
        Splitter,
        UseResgister
    }

    public enum EnumAggregate
    {
        None = 0,
        Sum,
        Count,
        Average
    }
}
