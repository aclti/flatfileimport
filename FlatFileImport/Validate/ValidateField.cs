﻿using System.Collections.Generic;
using System.Text;
using FlatFileImport.Exception;
using FlatFileImport.Process;
using System;

namespace FlatFileImport.Validate
{
    public class ValidateFieldPositionalCharacter : IValidate
    {
        private string[] _rawDataCollection;
        private IBlueprintLine _blueprintLine;

        public ValidateFieldPositionalCharacter(string[] rawDataCollection, IBlueprintLine blueprintLine)
        {
            // TODO: Complete member initialization
            _rawDataCollection = rawDataCollection;
            _blueprintLine = blueprintLine;
        }

        #region IValidate Members

        public bool IsValid()
        {
            var result = Valid();
            return result.Success;
        }

        public ValidResult Valid()
        {
            var result = new ValidResult { Success = true };
            //var sb = new StringBuilder();
            //var blueprintFields = _blueprintLine.BlueprintFields;

            //for (var i = 0; i < _rawDataCollection.Length; i++)
            //{
            //    //if (_rawDataCollection[i].Length > blueprintFields[i].Size && blueprintFields[i].Size > 0)
            //    //    sb.AppendFormat("[Classe: {0} | Campo: {1} | Valor: {4} | Tamanho: {2}  Esperado: {3}]\n", _blueprintLine.Class, blueprintFields[i].Attribute, _rawDataCollection[i].Length, blueprintFields[i].Size, _rawDataCollection[i]);

            //    if(_blueprintLine.BlueprintFields[i].Type == typeof(string) && _rawDataCollection[i].Length > blueprintFields[i].Size)
            //            sb.AppendFormat("[Classe: {0} | Campo: {1} | Valor: {4} | Tamanho: {2}  Esperado: {3}]\n", _blueprintLine.Class, blueprintFields[i].Attribute, _rawDataCollection[i].Length, blueprintFields[i].Size, _rawDataCollection[i]);
            //}
                

            //if (sb.Length > 0)
            //{
            //    result.Message = sb.ToString();
            //    result.Success = false;
            //    result.Severity = ExceptionSeverity.Fatal;
            //    result.Type = ExceptionType.Error;
            //}

            return result;
        }

        public ValidResult ValidateResult
        {
            get { return Valid(); }
        }

        #endregion
    }
}
