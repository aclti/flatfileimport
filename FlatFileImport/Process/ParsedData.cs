using System;

namespace FlatFileImport.Process
{
    //Verificar uma melhor forma encapsulamento
    public class ParsedData
    {
        private readonly Type _type;
        private readonly string _value;
        private readonly string _class;
        private readonly string _attribute;

        public Type Type { get { return _type; } }
        public string Value { get { return _value; } }
        public string Class { get { return _class; } }
        public string Attribute { get { return _attribute; } }

        public ParsedData(string classe, string attribute, string value, Type type)
        {
            _type = type;
            _value = value;
            _attribute = attribute;
            _class = classe;
        }
    }
}
