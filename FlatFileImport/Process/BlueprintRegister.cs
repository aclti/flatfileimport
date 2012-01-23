using System;
using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintRegister : IBlueprintRegister
    {
        public IBlueprint Blueprint { private set; get; }
        public string Class { set; get; }
        public Regex End { set; get; }
        public Regex Begin { set; get; }
        public bool IsComplet { set; get; }

        public BlueprintRegister(Blueprint blueprint)
        {
            if(blueprint == null)
                throw new ArgumentNullException("blueprint");

            Blueprint = blueprint;
        }
    }

    //<Registries>
    //    <Register name="D1000" inTransaction="true" begin="^D1000" end="^D9999" >
    //        <Query />
    //    </Register>
    //</Registries>
}
