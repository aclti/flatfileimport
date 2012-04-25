using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Data
{
    public class ParsedData : IParsedData, IParsedObjetct
    {
        private List<IParsedField> _fields;
        private List<IParsedData> _datas;
        private List<IParsedObjetct> _lines;

        public ParsedData(string name, IParsedData parent)
        {
            if(String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Name = name;
            Parent = parent;   
        }

        #region IParsedData,IParsedObjetct Members

        public IParsedData Parent { get; private set; }
        public string Name { get; private set; }

        public void AddField(string name, string value, Type type)
        {
            if (_fields == null)
                _fields = new List<IParsedField>();

            _fields.Add(new ParsedField(name, value, type, this));
        }

        #endregion

        #region IParsedData Members

        public ReadOnlyCollection<IParsedData> Headers { get { return _datas.AsReadOnly(); } }
        public ReadOnlyCollection<IParsedObjetct> Details { get { return _lines.AsReadOnly(); } }

        public void AddParsedData(string name)
        {
            if(_datas == null)
                _datas = new List<IParsedData>();

            _datas.Add(new ParsedData(name, this));
        }

        public void AddParsedData(IParsedData header)
        {
            if(header == null)
                throw new ArgumentNullException("header");

            if(header.Parent != this)
                throw new System.Exception("Pai errado.............");

            if (_datas == null)
                _datas = new List<IParsedData>();

            _datas.Add(header);
        }

        public void AddLine(IParsedObjetct details)
        {
            if (details == null)
                throw new ArgumentNullException("details");

            if (details.Parent != this)
                throw new System.Exception("Pai errado.............");

            if (_lines == null)
                _lines = new List<IParsedObjetct>();

            _lines.Add(details);
        }

        public void AddLine(string name)
        {
            if (_lines == null)
                _lines = new List<IParsedObjetct>();

            _lines.Add(new ParsedLine(name, this));
        }

        #endregion

        #region IParsedObjetct Members

        public ReadOnlyCollection<IParsedField> Fields { get { return _fields.AsReadOnly(); } }

        #endregion
    }
}