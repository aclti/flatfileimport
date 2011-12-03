using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class BlueprintRegister
    {
        public string Class { set; get; }
        public Regex End { set; get; }
        public Regex Begin { set; get; }
        public bool IsComplet { set; get; }
    }

//<Registries>
//    <Register name="D1000" inTransaction="true" begin="^D1000" end="^D9999" >
//        <Query />
//    </Register>
//</Registries>
}
