using System;
using FlatFileImport.Core;
using FlatFileImport.Exception;


namespace FlatFileImport.Validate
{
    public class ValidateField : IValidate
    {
        private string _rawData;
        private IBlueprintField _blueprintField;

        public ValidateField(string rawData, IBlueprintField blueprintField)
        {
            if (blueprintField == null)
                throw new ArgumentNullException("blueprintField");

            _rawData = rawData;
            _blueprintField = blueprintField;
        }

        #region IValidate Members

        public bool IsValid
        {
            get
            {
                var regex = _blueprintField.Regex;

                if (String.IsNullOrEmpty(_rawData))
                {
                    Result = new Result(_blueprintField.Name, _blueprintField.Type.Name, _rawData, "O campo importado não possui valor.", ExceptionType.Warnning, ExceptionSeverity.Information);
                    return false;
                }

                if (regex != null && !regex.Rule.IsMatch(_rawData))
                {
                    Result = new Result(_blueprintField.Name, _blueprintField.Regex.Name, _rawData, "O campo não casa com a Regex definida na Blueprint", ExceptionType.Error, ExceptionSeverity.Fatal);
                    return false;
                }
                    

                if (_blueprintField.Type == typeof(string) && _rawData.Length > _blueprintField.Size)
                {
                    Result = new Result(_blueprintField.Name, _blueprintField.Size.ToString(""), _rawData.Length.ToString(""), "O tamanho do campo é maior que o tamanho definido na Bluenprint" , ExceptionType.Error, ExceptionSeverity.Fatal);
                    return false;
                }

                return true;
            }
        }

        public IResult Result { get; private set; }

        #endregion

        //#region IValidate Members

        //public bool IsValid()
        //{
        //    var result = Valid();
        //    return result.Success;
        //}

        //public ValidResult Valid()
        //{
        //    var result = new ValidResult { Success = true };
        //    //var sb = new StringBuilder();
        //    //var blueprintFields = _blueprintLine.BlueprintFields;

        //    //for (var i = 0; i < _rawDataCollection.Length; i++)
        //    //{
        //    //    //if (_rawDataCollection[i].Length > blueprintFields[i].Size && blueprintFields[i].Size > 0)
        //    //    //    sb.AppendFormat("[Classe: {0} | Campo: {1} | Valor: {4} | Tamanho: {2}  Esperado: {3}]\n", _blueprintLine.Class, blueprintFields[i].Attribute, _rawDataCollection[i].Length, blueprintFields[i].Size, _rawDataCollection[i]);

        //    //    if(_blueprintLine.BlueprintFields[i].Type == typeof(string) && _rawDataCollection[i].Length > blueprintFields[i].Size)
        //    //            sb.AppendFormat("[Classe: {0} | Campo: {1} | Valor: {4} | Tamanho: {2}  Esperado: {3}]\n", _blueprintLine.Class, blueprintFields[i].Attribute, _rawDataCollection[i].Length, blueprintFields[i].Size, _rawDataCollection[i]);
        //    //}
                

        //    //if (sb.Length > 0)
        //    //{
        //    //    result.Message = sb.ToString();
        //    //    result.Success = false;
        //    //    result.Severity = ExceptionSeverity.Fatal;
        //    //    result.Type = ExceptionType.Error;
        //    //}

        //    return result;
        //}

        //public ValidResult ValidateResult
        //{
        //    get { return Valid(); }
        //}

        //#endregion
    }
}
