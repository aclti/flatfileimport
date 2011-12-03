using System.Text.RegularExpressions;

namespace FlatFileImport.Process
{
    public class RegexRule
    {
        private string _name;
        private Regex _rule;

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
    }
}
