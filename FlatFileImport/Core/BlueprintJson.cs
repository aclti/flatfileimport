namespace FlatFileImport.Core
{
    public class BlueprintJson : IBlueprint
    {
        public BlueprintJson(EnumFieldSeparationType separationType, char chaSeparator) { }

        #region IBlueprint Members

        public System.Collections.ObjectModel.ReadOnlyCollection<IBlueprintLine> BlueprintLines
        {
            get { throw new System.NotImplementedException(); }
        }

        public EnumFieldSeparationType FieldSeparationType
        {
            get { throw new System.NotImplementedException(); }
        }

        public char BluePrintCharSepartor
        {
            get { throw new System.NotImplementedException(); }
        }

        public void AddBlueprintLines(IBlueprintLine blueprintLine)
        {
            throw new System.NotImplementedException();
        }

        public void AddBlueprintLines(System.Collections.Generic.List<IBlueprintLine> blueprintLines)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
