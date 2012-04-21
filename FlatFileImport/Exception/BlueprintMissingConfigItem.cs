using System;

namespace FlatFileImport.Exception
{
    public class BlueprintMissingConfigItem : System.Exception, IImporterException
    {
        public BlueprintMissingConfigItem(string message) : base("O item não está presente ou não foi configurado corretamente: " + message)
        {
            
        }

        #region IParserException Members

        public ExceptionType Type
        {
            get { return ExceptionType.Error; }
        }

        public ExceptionSeverity Severity
        {
            get { return ExceptionSeverity.Fatal; }
        }

        #endregion
    }
}
