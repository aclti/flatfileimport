namespace FlatFileImport.Process
{
    public class ConverterString : Converter
    {
        //TODO: Utilizar de alguma forma os grupos para formatar a string;
        public override string Data
        {
            get { return RawData.Trim().Replace("'", "´"); }
        }
    }
}
