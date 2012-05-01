namespace FlatFileImport.Validate
{
    public interface IValidate
    {
        bool IsValid { get; }
        IResult Result { get; }
    }
}
