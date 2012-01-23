using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintLine : IBlueprintLine
    {
        public IBlueprint Blueprint { get; private set; }
        public string Class { set; get; }
        public Regex Regex { set; get; }
        public List<IBlueprintField> BlueprintFields { set; get; }
        public bool Mandatory { set; get; }

        public BlueprintLine(IBlueprint blueprint)
        {
            if(blueprint == null)
                throw new ArgumentNullException("blueprint");

            Blueprint = blueprint;
        }
    }
}