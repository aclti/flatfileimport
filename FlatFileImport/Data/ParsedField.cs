using System;

namespace FlatFileImport.Data
{
    public class ParsedField
    {
        private ParsedLine _parent;
        private string _name;
        private Type _type;
        private string _value;

        public ParsedLine Parent { get { return _parent; } }
        public Type Type { get { return _type; } }
        public string Value { get { return _value; } }
        public string Name { get { return _name; } }

        public ParsedField(string name, string value, Type type, ParsedLine line)
        {
            if (line == null)
                throw new ArgumentNullException("line");

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            _parent = line;
            _type = type;
            _value = value ?? "";
            _name = name;
        }
    }
}