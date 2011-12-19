using System;
using FlatFileImport.Input;
using FlatFileImport.Process;

namespace TestFlatFileImport
{
    public class BlueprintFactory : IBlueprintFactoy
    {
        #region IBlueprintFactoy Members

        public Blueprint GetBluePrint(Type type, FileInfo toParse)
        {
            return new Blueprint("");
        }

        #endregion
    }
}
