using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlatFileImport.Core
{
    public class BlueprintXml : IBlueprint
    {
        private List<IBlueprintLine> _line;
        private EnumFieldSeparationType _separationType;
        private char _chaSeparator;

        public BlueprintXml(EnumFieldSeparationType separationType, char chaSeparator)
        {
            _separationType = separationType;
            _chaSeparator = chaSeparator;
        }

        #region IBlueprint Members

        public IBlueprintLine Header
        {
            get { throw new NotImplementedException(); }
        }

        public IBlueprintLine Footer
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IBlueprintRegister> BlueprintRegistires
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IBlueprintLine> BlueprintLines
        {
            get { return _line.AsReadOnly(); }
        }

        public EnumFieldSeparationType FieldSeparationType
        {
            get { return _separationType; }
        }

        public char BluePrintCharSepartor
        {
            get { return _chaSeparator; }
        }

        public bool UseRegistries
        {
            get { throw new NotImplementedException(); }
        }

        public void AddBlueprintLines(IBlueprintLine blueprintLine)
        {
            if (_line == null)
                _line = new List<IBlueprintLine>();

            if (blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            _line.Add(blueprintLine);
        }

        public void AddBlueprintLines(List<IBlueprintLine> blueprintLines)
        {
            if (_line == null)
                _line = new List<IBlueprintLine>();

            if (blueprintLines == null)
                throw new ArgumentNullException("blueprintLines");

            _line.AddRange(blueprintLines);
        }

        #endregion
    }
}
