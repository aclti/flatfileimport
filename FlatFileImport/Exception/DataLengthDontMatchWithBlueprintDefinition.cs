using FlatFileImport.Validate;

namespace FlatFileImport.Exception
{
    public class DataLengthDontMatchWithBlueprintDefinition : System.Exception, IImporterException
    {
        public DataLengthDontMatchWithBlueprintDefinition(IResult result) 
            : base("A quantidade de campos definido na Blueprint não é igual a quantidade de campos do registro. Verifique a definição do registro na Blueprint.\n" + result.Message)
        {
            Severity = result.Severity;
            Type = result.Type;
        }

        public ExceptionType Type { get; private set; }
        public ExceptionSeverity Severity { get; private set; }
    }
}
