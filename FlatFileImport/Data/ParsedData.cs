using System.Collections.Generic;

namespace FlatFileImport.Data
{
    public class ParsedData
    {
        private ParsedData _parent;
        private string _name;
        private List<ParsedField> _fields;
        private List<ParsedData> _datas;
        private List<ParsedLine> _lines;
    }
}