﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Exception;
using FlatFileImport.Input;
using FlatFileImport.Validate;

namespace FlatFileImport.Process
{
    public class ParserPositional : IParser
    {
        private IRawLineAndFields _rawLine;
        private IBlueprintLine _blueprintLine;
        private IParsedObjetct _data;
        private Converter _converter;
        private IValidate _validate;
        private List<IResult> _results;

        public ParserPositional()
        {
            _results = new List<IResult>();
        }

        #region IParser Members

        public void SetDataToParse(IRawLineAndFields rawLine)
        {
            if (rawLine == null)
                throw new ArgumentNullException("rawLine");

            _rawLine = rawLine;

            foreach (var field in _blueprintLine.BlueprintFields)
                if (field.Position - 1 + field.Size <= _rawLine.Value.Length)
                    _rawLine.AddRawFiled(_rawLine.Value.Substring(field.Position - 1, field.Size));
        }

        public void SetBlueprintLine(IBlueprintLine blueprintLine)
        {
            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _blueprintLine = blueprintLine;
        }


        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedData(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            var rawFields = _rawLine.RawFields;
            _data = new ParsedData(_blueprintLine.Name, parent);

            for (var i = 0; i < _blueprintLine.BlueprintFields.Count; i++)
            {
                var data = rawFields[i].Value;
                var field = _blueprintLine.BlueprintFields[i];

                _converter = Converter.GetConvert(field.Type);
                _converter.Init(field, data);

                _data.AddField(field.Name, _converter.Data, field.Type);
            }

            return _data;
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public IParsedObjetct GetParsedLine(IParsedData parent)
        {
            HasBluprint();
            HasDataToParse();

            var rawFields = _rawLine.RawFields;
            _data = new ParsedLine(_blueprintLine.Name, parent);

            for (var i = 0; i < _blueprintLine.BlueprintFields.Count; i++)
            {
                var data = rawFields[i].Value;
                var field = _blueprintLine.BlueprintFields[i];

                _converter = Converter.GetConvert(field.Type);
                _converter.Init(field, data);

                _data.AddField(field.Name, _converter.Data, field.Type);
            }

            return _data;
        }

        public ReadOnlyCollection<IResult> Result
        {
            get
            {
                HasBluprint();
                HasDataToParse();
                return _results.AsReadOnly();
            }
        }

        // TODO: Migrar para um super classe ou para um command pattern
        public bool IsValid
        {
            get
            {
                HasBluprint();
                HasDataToParse();

                return ValidSintaxLine() && ValidSintaxAttribute();
            }
        }

        #endregion

        private void HasBluprint()
        {
            if (_blueprintLine == null)
                throw new System.Exception("Nenhum BluprintLine configurada para o parser.");
        }

        private void HasDataToParse()
        {
            if (_rawLine == null)
                throw new System.Exception("Nenhum Dado para ser analisado e importado.");
        }

        private bool ValidSintaxLine()
        {
            _validate = new ValidateLinePositional(_rawLine, _blueprintLine);

            if (!_validate.IsValid)
                _results.Add(_validate.Result);

            return _results.Count == 0 | _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }

        private bool ValidSintaxAttribute()
        {
            var rawFields = _rawLine.RawFields;

            for (var i = 0; i < _blueprintLine.BlueprintFields.Count; i++)
            {
                var field = _blueprintLine.BlueprintFields[i];
                var data = rawFields[i];

                _validate = new ValidateField(data, field);

                if (_validate.IsValid)
                    continue;

                _results.Add(_validate.Result);
            }

            return _results.Count == 0;// || _results.Count(r => r.Type == ExceptionType.Error) == 0;
        }
    }
}
