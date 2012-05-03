using System;

namespace FlatFileImport.Input
{
    public class RawField : IRawField
    {
        public RawField(string rawData, IRawLine parent)
        {
            if (rawData == null)
                throw new ArgumentNullException("rawData");

            if (parent == null)
                throw new ArgumentNullException("parent");

            Parent = parent;
            Value = rawData;
        }

        #region IRawField Members

        public IRawLine Parent { get; private set; }
        public string Value { get; private set; }

        #endregion
    }
}
