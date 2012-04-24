using System;

namespace FlatFileImport.Data
{
    public class ParsedField : IParsedField
    {
        #region IParsedField Members

        public IParsedObjetct Parent { get; private set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public string Value { get; private set; }

        #endregion

        public ParsedField(string name, string value, Type type, IParsedObjetct line)
        {
            if (line == null)
                throw new ArgumentNullException("line");

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            Parent = line;
            Type = type;
            Value = value ?? "";
            Name = name;
        }
    }
}