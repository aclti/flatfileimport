using System;

namespace FlatFileImport.Process
{
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

        public ParsedData(IBlueprintField blueprintField, string value)
        {
            if(blueprintField == null)
                throw new ArgumentNullException("blueprintField");

            _type = blueprintField.Type;
            _value = value;
            _attribute = blueprintField.Attribute;
            _class = blueprintField.BlueprintLine.Class;
        }
    }
}
