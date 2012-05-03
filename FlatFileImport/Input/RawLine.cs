using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Input
{
    public class RawLine : IRawLine
    {
        private List<IRawField> _rawFields;

        public RawLine(int lineNumber, string rawData)
        {
            if (lineNumber <= 0)
                throw new System.Exception("Numero da linha inválido.");

            if (String.IsNullOrEmpty(rawData))
                throw new ArgumentNullException("rawData");

            Number = lineNumber;
            Value = rawData;

            _rawFields = new List<IRawField>();
        }

        #region IRawLine Members

        public int Number { get; private set; }
        public string Value { get; private set; }
        public ReadOnlyCollection<IRawField> RawFields { get { return _rawFields.AsReadOnly(); } }

        public void AddRawFiled(string rawValue)
        {
            _rawFields.Add(new RawField(rawValue, this));
        }

        #endregion
    }
}
