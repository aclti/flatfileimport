using System.Collections.Generic;
using FlatFileImport.Core;
using FlatFileImport.Data;

namespace FlatFileImport.Validate
{
    public class ValidateRegister : IValidate
    {
        private List<ParsedLine> _parsedDatas;

        public ValidateRegister(List<ParsedLine> parsedDatas, List<IBlueprintField> blueprintFields)
        {
            // TODO: Complete member initialization
            _parsedDatas = parsedDatas;
        }

        #region IValidate Members

        public bool IsValid()
        {
            throw new System.NotImplementedException();
        }

        public ValidResult Valid()
        {
            throw new System.NotImplementedException();
        }

        public ValidResult ValidateResult
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
