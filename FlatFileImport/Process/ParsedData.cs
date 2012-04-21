using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Process
{
    public class ParsedData
    {
        private  List<ParsedAttribute> _attributes;
        private bool _mandatory;
        private string _name;

        public string Name { get { return _name; } }
        public bool Mandatory{ get { return _mandatory; } }

        public ReadOnlyCollection<ParsedAttribute> Attributes { get { return _attributes.AsReadOnly(); } }

        public ParsedData(string name, bool mandatory)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _mandatory = mandatory;
            _name = name;
        }

        public void AddAtributte(string name, string value, Type type)
        {
            var attribute = new ParsedAttribute(name, value, type, this);

            if(_attributes == null)
                _attributes = new List<ParsedAttribute>();

            _attributes.Add(attribute);
        }
    }
}
