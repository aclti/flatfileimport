using System.Collections.Generic;
using FlatFileImport.Process;

namespace FlatFileImport.Validate
{
    public class ValidateRegister : IValidate
    {
        private List<ParsedData> _parsedDatas;

        public ValidateRegister(List<ParsedData> parsedDatas, List<IBlueprintField> blueprintFields)
        {
            // TODO: Complete member initialization
            _parsedDatas = parsedDatas;
        }

        #region IValidate Members

        public bool IsValid()
        {
            throw new System.NotImplementedException();
        }

        public ValidateResult Valid()
        {
            throw new System.NotImplementedException();
        }

        public ValidateResult ValidateResult
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
