using System;
using FlatFileImport.Core;
using FlatFileImport.Exception;
using FlatFileImport.Input;


namespace FlatFileImport.Validate
{
    public class ValidateField : IValidate
    {
        private IRawField _rawData;
        private IBlueprintField _blueprintField;

        public ValidateField(IRawField rawData, IBlueprintField blueprintField)
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

                if (String.IsNullOrEmpty(_rawData.Value))
                {
                    Result = new Result("O campo importado não possui valor.", ExceptionType.Warnning, ExceptionSeverity.Information)
                                 {
                                     LineName = _blueprintField.Parent.Name,
                                     LineNumber = _rawData.Parent.Number,
                                     FieldName =_blueprintField.Name,
                                     Value = _rawData.Value,
                                     Expected = _blueprintField.Type.Name,
                                 };

                    return false;
                }

                if (regex != null && !regex.Rule.IsMatch(_rawData.Value))
                {
                    Result = new Result("O campo não casa com a Regex definida na Blueprint", ExceptionType.Error, ExceptionSeverity.Fatal)
                                 {
                                     LineName = _blueprintField.Parent.Name,
                                     LineNumber = _rawData.Parent.Number,
                                     FieldName =_blueprintField.Name, 
                                     Value = _rawData.Value,
                                     Expected = _blueprintField.Regex.Name,
                                 };

                    return false;
                }
                    

                if (_blueprintField.Type == typeof(string) && _rawData.Value.Length > _blueprintField.Size)
                {
                    Result = new Result("O tamanho do campo é maior que o tamanho definido na Bluenprint" , ExceptionType.Error, ExceptionSeverity.Fatal)
                                 {
                                     LineName = _blueprintField.Parent.Name,
                                     LineNumber = _rawData.Parent.Number,
                                     FieldName =_blueprintField.Name, 
                                     Value = _rawData.Value.Length.ToString(""),
                                     Expected = _blueprintField.Size.ToString(""),
                                 };

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
