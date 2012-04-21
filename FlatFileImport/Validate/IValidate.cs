namespace FlatFileImport.Validate
{
    public interface IValidate
    {
        bool IsValid();
        ValidResult Valid();
        ValidResult ValidateResult { get; }
    }
}
