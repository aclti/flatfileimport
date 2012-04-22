using System;
using System.Linq;
using System.Text.RegularExpressions;
using FlatFileImport.Core;

namespace FlatFileImport.Process
{
    public abstract class Converter
    {
        protected IBlueprintField BlueprintField { get; private set; }
        protected RegexRule RegexRule { get { return BlueprintField.Regex; } }
        protected EnumFieldSeparationType FieldSeparationType { get { return BlueprintField.Parent.Blueprint.FieldSeparationType; } }
        protected bool HasGroup;
        protected Match Match;
        protected string RawData;

        public abstract string Data { get; }

        public void Init(IBlueprintField blueprintField, string rawData)
        {
            BlueprintField = blueprintField;

            if(RegexRule != null)
                HasGroup = RegexRule.Rule.GetGroupNames().Length > 0 ;

            RawData = rawData;
        }

        protected bool HasGroupMember(string memberName)
        {
            var group = RegexRule.Rule.GetGroupNames();
            return group.Any(member => member == memberName);
        }

        public static Converter GetConvert(Type type)
        {
            if (type == typeof(decimal) || type == typeof(int))
                return new ConverterNumber();

            if (type == typeof(DateTime))
                return new ConverterDate();

            if (type == typeof(string))
                return new ConverterString();

            throw new System.Exception("Conversor não implementado");
        }
    }
}
