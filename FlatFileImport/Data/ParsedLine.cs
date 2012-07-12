using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Data
{
    public class ParsedLine : IParsedObjetct
    {
        private  List<IParsedField> _fields;

        public IParsedData Parent { get; private set; }
        public string Name { get; private set; }
        public ReadOnlyCollection<IParsedField> Fields { get { return _fields.AsReadOnly(); } }

        public ParsedLine(string name, IParsedData parent)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            //if (parent == null)
            //    throw new ArgumentNullException("parent");

            _fields = new List<IParsedField>();

            Name = name;
            Parent = parent;
        }

        public void AddField(string name, string value, Type type)
        {
            var field = new ParsedField(name, value, type, this);

            _fields.Add(field);
        }
    }
}