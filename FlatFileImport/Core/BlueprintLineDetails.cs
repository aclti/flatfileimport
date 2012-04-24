using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FlatFileImport.Aggregate;

namespace FlatFileImport.Core
{
    public class BlueprintLineDetails : IBlueprintLine, IAggregateSubject
    {
        public BlueprintLineDetails(IBlueprint blueprint, IBlueprintLine parent)
        {
            if (blueprint == null)
                throw new ArgumentNullException("blueprint");

            if (parent == null)
                throw new ArgumentNullException("parent");

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
