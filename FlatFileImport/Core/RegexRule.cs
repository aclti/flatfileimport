using System.Text.RegularExpressions;

namespace FlatFileImport.Core
{
    public class RegexRule
    {
        private readonly string _name;
        private readonly Regex _rule;

        public RegexRule(string name, string rule)
        {
            _name = name;
            _rule = new Regex(rule);
        }

        public string Name
        {
            get { return _name; }
        }

        public Regex Rule
        {
            get { return _rule; }
        }

        //private string DecodeString(string toDecode)
        //{
        //    return toDecode.Replace("&lt;", "<").Replace("&gt;", ">");
        //}
    }
}
