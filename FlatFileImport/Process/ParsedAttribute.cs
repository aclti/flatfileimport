using System;

namespace FlatFileImport.Process
{
    public class ParsedAttribute
    {
        private readonly ParsedData _class;
        private Type _type;
        private string _value;
        private string _name;

        public ParsedData Class { get { return _class; } }
        public Type Type { get { return _type; } }
        public string Value { get { return _value; } }
        public string Name { get { return _name; } }

        public ParsedAttribute(string name, string value, Type type, ParsedData classe)
        {
            if (classe == null)
                throw new ArgumentNullException("classe");

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            _class = classe;
            _type = type;
            _value = value ?? "";
            _name = name;
        }
    }
}
