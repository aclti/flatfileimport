using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileImport.Core
{
    public class BlueprintLineHeader : IBlueprintLine
    {
        public BlueprintLineHeader(IBlueprint blueprint, IBlueprintLine parent)
        {
            if (blueprint == null)
                throw new ArgumentNullException("blueprint");

            Blueprint = blueprint;
            Parent = parent;
        }

        #region IBlueprintLine Members

        public IBlueprint Blueprint { get; private set; }
        public IBlueprintLine Parent { get; private set; }
        public string Name { get; set; }
        public Regex Regex{get;set;}
        public List<IBlueprintField> BlueprintFields{get; set; }
        public IOccurrence Occurrence{get;set;}

        #endregion
    }
}
