using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Data
{
    public class ParsedLine
    {
        private ParsedData _parent;
        private string _name;
        private  List<ParsedField> _fields;

        public string Name { get { return _name; } }

        public ReadOnlyCollection<ParsedField> Fields { get { return _fields.AsReadOnly(); } }

        public ParsedLine(string name, bool mandatory)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }

        public void AddField(string name, string value, Type type)
        {
            var field = new ParsedField(name, value, type, this);

            if(_fields == null)
                _fields = new List<ParsedField>();

            _fields.Add(field);
        }
    }
}