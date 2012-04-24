using System;

namespace FlatFileImport.Core
{
    public class BlueprintField : IBlueprintField
    {
        public IBlueprintLine Parent { private set; get; }
        public string Name { set; get; }
        public Type Type { set; get; }
        public int Size { set; get; }
        public int Precision { set; get; }
        public RegexRule Regex { set; get; }
        public bool Persist { set; get; }
        public int Position { get; set; }

        public BlueprintField(IBlueprintLine blueprintLine)
        {
            if(blueprintLine ==  null)
                throw new ArgumentNullException("blueprintLine");

            Parent = blueprintLine;
        }
    }
}
